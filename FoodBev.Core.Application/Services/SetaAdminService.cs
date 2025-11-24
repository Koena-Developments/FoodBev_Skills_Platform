using FoodBev.Application.DTOs.AdminManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the business logic for SETA administrative tasks.
    /// </summary>
    public class SetaAdminService : ISetaAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetaAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --- Helper Methods ---

        private OfoCode MapToEntity(CreateOfoCodeDto dto)
        {
            return new OfoCode
            {
                Code = dto.Code,
                Description = dto.Description,
                Sector = dto.Sector
            };
        }

        private OfoCodeDto MapToDto(OfoCode entity)
        {
            return new OfoCodeDto
            {
                OfoCodeID = entity.OfoCodeID,
                Code = entity.Code,
                Description = entity.Description,
                Sector = entity.Sector
            };
        }
        
        // --- OFO Code Management ---

        public async Task<OfoCodeDto> CreateOfoCodeAsync(CreateOfoCodeDto dto)
        {
            var ofoCode = MapToEntity(dto);
            await _unitOfWork.OfoCodes.AddAsync(ofoCode);
            await _unitOfWork.CompleteAsync();
            return MapToDto(ofoCode);
        }

        public async Task<IEnumerable<OfoCodeDto>> GetAllOfoCodesAsync()
        {
            var codes = await _unitOfWork.OfoCodes.GetAllAsync();
            return codes.Select(MapToDto);
        }

        public async Task<OfoCodeDto> GetOfoCodeByIdAsync(int ofoCodeId)
        {
            var code = await _unitOfWork.OfoCodes.GetByIdAsync(ofoCodeId);
            return MapToDto(code);
        }

        public async Task<OfoCodeDto> UpdateOfoCodeAsync(int ofoCodeId, CreateOfoCodeDto dto)
        {
            var code = await _unitOfWork.OfoCodes.GetByIdAsync(ofoCodeId);
            if (code == null) return null;

            code.Code = dto.Code;
            code.Description = dto.Description;
            code.Sector = dto.Sector;

            _unitOfWork.OfoCodes.Update(code);
            await _unitOfWork.CompleteAsync();
            return MapToDto(code);
        }

        public async Task<bool> DeleteOfoCodeAsync(int ofoCodeId)
        {
            var code = await _unitOfWork.OfoCodes.GetByIdAsync(ofoCodeId);
            if (code == null) return false;

            _unitOfWork.OfoCodes.Delete(code);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // --- Application Vetting ---

        public async Task<IEnumerable<ApplicationReviewDto>> GetPendingApplicationsAsync()
        {
            // Get applications by status which includes navigation properties
            var applications = await _unitOfWork.Applications.GetApplicationsByStatusAsync(ApplicationStatus.Submitted);

            var reviewList = new List<ApplicationReviewDto>();
            foreach (var app in applications)
            {
                var candidate = await _unitOfWork.Candidates.GetByIdAsync(app.CandidateID);
                var employer = await _unitOfWork.Employers.GetByIdAsync(app.EmployerID);
                // SkillsProgrammeID doesn't exist on ApplicationEntity - use SkillsProgrammeForm if available
                var skillsForm = app.SkillsProgrammeForm;

                reviewList.Add(new ApplicationReviewDto
                {
                    ApplicationID = app.ApplicationID,
                    CandidateID = app.CandidateID,
                    EmployerID = app.EmployerID,
                    SkillsProgrammeID = skillsForm?.FormID, // Use FormID if form exists
                    Status = app.Status,
                    SubmissionDate = app.DateApplied, // ✅ Use existing DateApplied
                    EnrollmentFormDocumentRef = app.EnrollmentFormDocumentRef ?? string.Empty,

                    CandidateFullName = $"{candidate?.FirstName ?? ""} {candidate?.LastName ?? ""}",
                    EmployerCompanyName = employer?.CompanyName ?? "N/A",
                    SkillsProgrammeTitle = skillsForm != null ? $"Skills Programme Form #{skillsForm.FormID}" : "N/A"
                });
            }
            return reviewList;
        }

        public async Task<ApplicationReviewDto> GetApplicationForReviewAsync(int applicationId)
        {
            var app = await _unitOfWork.Applications.GetApplicationWithDetailsAsync(applicationId);
            if (app == null) return null;

            var candidate = await _unitOfWork.Candidates.GetByIdAsync(app.CandidateID);
            var employer = await _unitOfWork.Employers.GetByIdAsync(app.EmployerID);
            // SkillsProgrammeID doesn't exist on ApplicationEntity - use SkillsProgrammeForm if available
            var skillsForm = app.SkillsProgrammeForm;

            return new ApplicationReviewDto
            {
                ApplicationID = app.ApplicationID,
                CandidateID = app.CandidateID,
                EmployerID = app.EmployerID,
                SkillsProgrammeID = skillsForm?.FormID, // Use FormID if form exists
                Status = app.Status,
                SubmissionDate = app.DateApplied,
                EnrollmentFormDocumentRef = app.EnrollmentFormDocumentRef ?? string.Empty,

                CandidateFullName = $"{candidate?.FirstName ?? ""} {candidate?.LastName ?? ""}",
                EmployerCompanyName = employer?.CompanyName ?? "N/A",
                SkillsProgrammeTitle = skillsForm != null ? $"Skills Programme Form #{skillsForm.FormID}" : "N/A"
            };
        }

        public async Task<ApplicationReviewDto> UpdateApplicationStatusAsync(int applicationId, UpdateApplicationStatusDto dto)
        {
            var application = await _unitOfWork.Applications.GetByIdAsync(applicationId);
            if (application == null) return null;

            application.Status = dto.NewStatus;
            application.AdminNotes = dto.AdminNotes ?? string.Empty;

            if (dto.NewStatus == ApplicationStatus.Registered)
            {
                application.RegistrationNumber = dto.RegistrationNumber ?? string.Empty;
                application.DateOfRegistration = dto.RegistrationDate ?? System.DateTime.UtcNow;
                application.RegisteredBy = dto.RegisteredBy ?? string.Empty;
            }

            _unitOfWork.Applications.Update(application);
            await _unitOfWork.CompleteAsync();

            return await GetApplicationForReviewAsync(applicationId);
        }
    }
}