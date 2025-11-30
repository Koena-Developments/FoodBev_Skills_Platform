using FoodBev.Application.DTOs.TripartiteAgreement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for Tripartite Agreement management.
    /// </summary>
    public class TripartiteAgreementService : ITripartiteAgreementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEmployerService _employerService;

        public TripartiteAgreementService(IUnitOfWork unitOfWork, IFileStorageService fileStorageService, IEmployerService employerService)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _employerService = employerService;
        }

        public async Task<TripartiteAgreementDto> CreateAgreementAsync(int applicationId)
        {
            // Check if agreement already exists
            var existing = await _unitOfWork.TripartiteAgreements.GetByApplicationIdAsync(applicationId);
            if (existing != null)
            {
                return await MapToDto(existing);
            }

            // Verify application exists
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new InvalidOperationException("Application not found.");
            }

            // Create new agreement
            var agreement = new TripartiteAgreement
            {
                ApplicationID = applicationId,
                Status = TripartiteAgreementStatus.PendingCandidateSignature,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.TripartiteAgreements.AddAsync(agreement);
            await _unitOfWork.CompleteAsync();

            return await MapToDto(agreement);
        }

        public async Task<TripartiteAgreementDto?> GetAgreementByIdAsync(int agreementId)
        {
            var agreement = await _unitOfWork.TripartiteAgreements.GetByIdAsync(agreementId);
            return agreement != null ? await MapToDto(agreement) : null;
        }

        public async Task<TripartiteAgreementDto?> GetAgreementByApplicationIdAsync(int applicationId)
        {
            var agreement = await _unitOfWork.TripartiteAgreements.GetByApplicationIdAsync(applicationId);
            return agreement != null ? await MapToDto(agreement) : null;
        }

        public async Task<IEnumerable<TripartiteAgreementDto>> GetAgreementsByCandidateIdAsync(int candidateId)
        {
            var agreements = await _unitOfWork.TripartiteAgreements.GetByCandidateIdAsync(candidateId);
            var dtos = new List<TripartiteAgreementDto>();
            
            foreach (var agreement in agreements)
            {
                dtos.Add(await MapToDto(agreement));
            }
            
            return dtos;
        }

        public async Task<IEnumerable<TripartiteAgreementDto>> GetAgreementsByEmployerIdAsync(int employerId)
        {
            var agreements = await _unitOfWork.TripartiteAgreements.GetByEmployerIdAsync(employerId);
            var dtos = new List<TripartiteAgreementDto>();
            
            foreach (var agreement in agreements)
            {
                dtos.Add(await MapToDto(agreement));
            }
            
            return dtos;
        }

        public async Task<IEnumerable<TripartiteAgreementDto>> GetPendingAdminReviewAsync()
        {
            var agreements = await _unitOfWork.TripartiteAgreements.GetPendingAdminReviewAsync();
            var dtos = new List<TripartiteAgreementDto>();
            
            foreach (var agreement in agreements)
            {
                dtos.Add(await MapToDto(agreement));
            }
            
            return dtos;
        }

        public async Task<bool> SignAsCandidateAsync(int agreementId, string userId, string signatureBase64)
        {
            var agreement = await _unitOfWork.TripartiteAgreements.GetByIdAsync(agreementId);
            if (agreement == null || agreement.Status != TripartiteAgreementStatus.PendingCandidateSignature)
            {
                return false;
            }

            // Verify the user is the candidate for this application
            var application = await _unitOfWork.Applications.GetByIdAsync(agreement.ApplicationID);
            if (application == null || application.CandidateID.ToString() != userId)
            {
                return false;
            }

            agreement.CandidateSignature = signatureBase64;
            agreement.CandidateSignedDate = DateTime.UtcNow;
            agreement.CandidateSignedByUserID = int.Parse(userId);
            agreement.Status = TripartiteAgreementStatus.AwaitingEmployerSignature;

            _unitOfWork.TripartiteAgreements.Update(agreement);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> SignAsEmployerAsync(int agreementId, string userId, string employerSignatureBase64, string? trainingProviderSignatureFileRef)
        {
            var agreement = await _unitOfWork.TripartiteAgreements.GetByIdAsync(agreementId);
            if (agreement == null || agreement.Status != TripartiteAgreementStatus.AwaitingEmployerSignature)
            {
                return false;
            }

            // Verify the user is the employer for this application
            var application = await _unitOfWork.Applications.GetByIdAsync(agreement.ApplicationID);
            if (application == null)
            {
                return false;
            }

            var job = await _unitOfWork.JobPostings.GetByIdAsync(application.JobID);
            if (job == null)
            {
                return false;
            }

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue || employerId.Value != job.EmployerID)
            {
                return false;
            }

            agreement.EmployerSignature = employerSignatureBase64;
            agreement.EmployerSignedDate = DateTime.UtcNow;
            agreement.EmployerSignedByUserID = int.Parse(userId);

            if (!string.IsNullOrEmpty(trainingProviderSignatureFileRef))
            {
                agreement.TrainingProviderSignatureFileRef = trainingProviderSignatureFileRef;
                agreement.TrainingProviderSignatureUploadDate = DateTime.UtcNow;
                agreement.TrainingProviderSignatureUploadedByUserID = int.Parse(userId);
            }

            // Only move to SubmittedToAdmin if both employer and TP signatures are present
            if (!string.IsNullOrEmpty(agreement.EmployerSignature) && !string.IsNullOrEmpty(agreement.TrainingProviderSignatureFileRef))
            {
                agreement.Status = TripartiteAgreementStatus.SubmittedToAdmin;
                agreement.SubmittedToAdminDate = DateTime.UtcNow;
            }

            _unitOfWork.TripartiteAgreements.Update(agreement);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> ReviewAgreementAsync(int agreementId, string userId, bool approved, string? notes)
        {
            var agreement = await _unitOfWork.TripartiteAgreements.GetByIdAsync(agreementId);
            if (agreement == null || agreement.Status != TripartiteAgreementStatus.SubmittedToAdmin)
            {
                return false;
            }

            agreement.Status = approved ? TripartiteAgreementStatus.Approved : TripartiteAgreementStatus.Rejected;
            agreement.AdminReviewedDate = DateTime.UtcNow;
            agreement.AdminReviewedByUserID = int.Parse(userId);
            agreement.AdminNotes = notes;

            _unitOfWork.TripartiteAgreements.Update(agreement);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        private async Task<TripartiteAgreementDto> MapToDto(TripartiteAgreement agreement)
        {
            var application = agreement.Application ?? await _unitOfWork.Applications.GetByIdAsync(agreement.ApplicationID);
            if (application == null)
            {
                throw new InvalidOperationException("Application not found for agreement.");
            }

            var job = application.Job ?? await _unitOfWork.JobPostings.GetByIdAsync(application.JobID);
            var candidate = application.Candidate ?? await _unitOfWork.Candidates.GetByIdAsync(application.CandidateID);
            var employer = job != null ? await _unitOfWork.Employers.GetByIdAsync(job.EmployerID) : null;

            return new TripartiteAgreementDto
            {
                AgreementID = agreement.AgreementID,
                ApplicationID = agreement.ApplicationID,
                Status = agreement.Status,
                CreatedDate = agreement.CreatedDate,
                HasCandidateSignature = !string.IsNullOrEmpty(agreement.CandidateSignature),
                CandidateSignedDate = agreement.CandidateSignedDate,
                CandidateSignature = agreement.CandidateSignature, // Include for admin view
                HasEmployerSignature = !string.IsNullOrEmpty(agreement.EmployerSignature),
                EmployerSignedDate = agreement.EmployerSignedDate,
                EmployerSignature = agreement.EmployerSignature, // Include for admin view
                HasTrainingProviderSignature = !string.IsNullOrEmpty(agreement.TrainingProviderSignatureFileRef),
                TrainingProviderSignatureUploadDate = agreement.TrainingProviderSignatureUploadDate,
                TrainingProviderSignatureFileRef = agreement.TrainingProviderSignatureFileRef, // Include for admin view
                SubmittedToAdminDate = agreement.SubmittedToAdminDate,
                AdminReviewedDate = agreement.AdminReviewedDate,
                AdminNotes = agreement.AdminNotes,
                JobID = job?.JobID ?? 0,
                JobTitle = job?.JobTitle ?? "Unknown",
                CandidateID = candidate?.CandidateID ?? 0,
                CandidateName = $"{candidate?.FirstName} {candidate?.LastName}".Trim(),
                EmployerID = employer?.EmployerID ?? 0,
                EmployerCompanyName = employer?.CompanyName ?? "Unknown"
            };
        }
    }
}

