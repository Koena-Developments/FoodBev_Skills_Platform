using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities; // 👈 WAS MISSING — now added
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for application and skills form management.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Helper method to enrich application entity with job/employer data and map to DTO
        private async Task<ApplicationSummaryDto> MapToSummaryDto(ApplicationEntity application)
        {
            if (application == null) return null;

            // Fetch related details needed for the summary (Job and Employer)
            var job = application.Job ?? await _unitOfWork.JobPostings.GetByIdAsync(application.JobID);
            var employer = job != null ? await _unitOfWork.Employers.GetByIdAsync(job.EmployerID) : null;
            
            // Check if the Skills Form exists
            var form = await _unitOfWork.SkillsProgrammeForms.GetByApplicationIdAsync(application.ApplicationID);
            
            return new ApplicationSummaryDto
            {
                ApplicationID = application.ApplicationID,
                JobID = application.JobID,
                CandidateID = application.CandidateID,
                JobTitle = job?.JobTitle ?? "Job Not Found",
                CompanyName = employer?.CompanyName ?? "Company Not Found",
                DateApplied = application.DateApplied,
                Status = application.Status,
                InterviewDate = application.InterviewDate,
                InterviewVenue = application.InterviewVenue,
                InterviewResponse = application.InterviewResponse,
                HasSkillsForm = form != null // Indicate if the form linked to the application exists
            };
        }
        
        // Helper method to map form entity to DTO
        private SkillsFormDto MapToFormDto(SkillsProgrammeForm form)
        {
            if (form == null) return null;

            return new SkillsFormDto
            {
                FormID = form.FormID,
                ApplicationID = form.ApplicationID,
                Status = form.Status,
                CandidateSignatureStatus = string.IsNullOrWhiteSpace(form.CandidateSignature) ? "Pending" : "Signed",
                EmployerSignatureStatus = string.IsNullOrWhiteSpace(form.EmployerSignature) ? "Pending" : "Signed",
                AdminRegistrationNo = form.AdminRegistrationNo,
                DateSubmitted = form.DateSubmitted
            };
        }

        public async Task<ApplicationSummaryDto> ApplyToJobAsync(CreateApplicationDto dto)
        {
            // 1. Validation: Check if candidate and job exist
            var job = await _unitOfWork.JobPostings.GetByIdAsync(dto.JobID);
            var candidate = await _unitOfWork.Candidates.GetByIdAsync(dto.CandidateID);

            if (job == null || candidate == null)
            {
                return null; // Job or Candidate not found
            }
            
            // 2. Create Application Entity
            var application = new ApplicationEntity
            {
                JobID = dto.JobID,
                CandidateID = dto.CandidateID,
                CV_File_Ref = dto.CV_File_Ref,
                CandidateAvailability = dto.CandidateAvailability,
                Status = ApplicationStatus.Applied,
            };

            // This relies on the unique index on (JobID, CandidateID) in the DbContext to prevent duplicates
            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.CompleteAsync();

            // 3. Create initial Skills Programme Form (one-to-one relationship)
            var form = new SkillsProgrammeForm
            {
                ApplicationID = application.ApplicationID,
                DateSubmitted = application.DateApplied,
                Status = FormStatus.Pending_Candidate // Form starts by waiting for the candidate to sign
            };
            
            await _unitOfWork.SkillsProgrammeForms.AddAsync(form);
            await _unitOfWork.CompleteAsync();

            // 4. Return Summary DTO
            return await MapToSummaryDto(application);
        }

        public async Task<IEnumerable<ApplicationSummaryDto>> GetApplicationsByCandidateAsync(int candidateId)
        {
            // The repository loads applications linked to jobs
            var applications = await _unitOfWork.Applications.GetApplicationsByCandidateAsync(candidateId);
            
            var dtos = new List<ApplicationSummaryDto>();
            foreach (var app in applications)
            {
                dtos.Add(await MapToSummaryDto(app));
            }
            return dtos;
        }

        public async Task<IEnumerable<ApplicationSummaryDto>> GetApplicationsByJobAsync(int jobId)
        {
            // The repository loads applications linked to candidates
            var applications = await _unitOfWork.Applications.GetApplicationsByJobAsync(jobId);
            
            var dtos = new List<ApplicationSummaryDto>();
            foreach (var app in applications)
            {
                dtos.Add(await MapToSummaryDto(app));
            }
            return dtos;
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);

            if (application == null)
            {
                return false;
            }

            application.Status = newStatus;
            
            // If we move to 'Hired', we might need to trigger notifications or final checks
            if (newStatus == ApplicationStatus.Hired)
            {
                // Business logic: Ensure form is fully signed before marking as Hired if necessary.
                // For now, we allow the status update to proceed.
            }
            
            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }

        public async Task<SkillsFormDto> GetSkillsProgrammeFormAsync(int applicationId)
        {
            var form = await _unitOfWork.SkillsProgrammeForms.GetByApplicationIdAsync(applicationId);
            return MapToFormDto(form);
        }

        public async Task<bool> SubmitCandidateSignatureAsync(SignatureDto signatureDto)
        {
            var form = await _unitOfWork.SkillsProgrammeForms.GetByApplicationIdAsync(signatureDto.ApplicationID);
            
            if (form == null || form.Status != FormStatus.Pending_Candidate)
            {
                return false; // Form not found or not in the correct state for candidate signature
            }

            form.CandidateSignature = signatureDto.SignatureBase64;
            form.Status = FormStatus.Pending_Employer; // Move to the employer signing stage
            
            _unitOfWork.SkillsProgrammeForms.Update(form);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }

        public async Task<bool> SubmitEmployerSignatureAsync(SignatureDto signatureDto)
        {
            var form = await _unitOfWork.SkillsProgrammeForms.GetByApplicationIdAsync(signatureDto.ApplicationID);
            
            if (form == null || form.Status != FormStatus.Pending_Employer)
            {
                return false; // Form not found or not in the correct state for employer signature
            }
            
            form.EmployerSignature = signatureDto.SignatureBase64;
            form.Status = FormStatus.Pending_Admin; // Move to the SETA Admin approval stage
            
            _unitOfWork.SkillsProgrammeForms.Update(form);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }

        public async Task<bool> ScheduleInterviewAsync(int applicationId, DateTime interviewDate, string interviewVenue)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
            if (application == null)
            {
                return false;
            }

            application.InterviewDate = interviewDate;
            application.InterviewVenue = interviewVenue;
            application.Status = ApplicationStatus.InterviewScheduled;
            application.InterviewResponse = InterviewResponse.None; // Reset response
            
            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }

        public async Task<bool> UpdateInterviewResponseAsync(int applicationId, InterviewResponse response)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
            if (application == null || application.Status != ApplicationStatus.InterviewScheduled)
            {
                return false;
            }

            application.InterviewResponse = response;
            
            // Update status based on response
            if (response == InterviewResponse.Accepted)
            {
                application.Status = ApplicationStatus.InterviewAccepted;
            }
            else if (response == InterviewResponse.Declined)
            {
                application.Status = ApplicationStatus.InterviewDeclined;
            }
            
            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }

        public async Task<IEnumerable<ApplicationSummaryDto>> GetFilteredApplicantsForJobAsync(int jobId, string? ofoCode = null, string? employmentStatus = null, string? province = null)
        {
            var applications = await _unitOfWork.Applications.GetApplicationsByJobAsync(jobId);
            
            // Apply filters
            var filtered = applications.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(ofoCode))
            {
                // Filter by candidate's OFO code
                filtered = filtered.Where(a => a.Candidate.OFO_Code == ofoCode);
            }
            
            if (!string.IsNullOrWhiteSpace(employmentStatus))
            {
                // Filter by candidate's employment status
                filtered = filtered.Where(a => a.Candidate.EmploymentStatus == employmentStatus);
            }
            
            if (!string.IsNullOrWhiteSpace(province))
            {
                // Filter by candidate's province
                filtered = filtered.Where(a => a.Candidate.Province == province);
            }
            
            var dtos = new List<ApplicationSummaryDto>();
            foreach (var app in filtered)
            {
                dtos.Add(await MapToSummaryDto(app));
            }
            
            return dtos;
        }
    }
}