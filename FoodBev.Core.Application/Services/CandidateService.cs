using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for managing Candidate profiles.
    /// </summary>
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private CandidateProfileDto MapToDto(CandidateEntity candidate)
        {
            if (candidate == null) return null;

            return new CandidateProfileDto
            {
                CandidateID = candidate.CandidateID,
                UserID = candidate.CandidateID.ToString(),
                
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                IDNumber = candidate.IDNumber,
                DateOfBirth = candidate.DateOfBirth ?? default(DateTime),
                Race = candidate.Race,
                Gender = candidate.Gender,
                IsDisabled = candidate.IsDisabled,
                DisabilityDetails = candidate.DisabilityDetails,
                Nationality = candidate.Nationality,
                
                ContactNumber = candidate.ContactNumber,
                Email = candidate.Email,
                PhysicalAddress = candidate.PhysicalAddress,
                PostalCode = candidate.PostalCode,
                Province = candidate.Province,
                
                HighestQualification = candidate.HighestQualification,
                InstitutionName = candidate.InstitutionName,
                QualificationYear = candidate.QualificationYear ?? 0,
                EmploymentStatus = candidate.EmploymentStatus,
                OFO_Code = candidate.OFO_Code,
                AcceptsPOPI = candidate.AcceptsPOPI,
                
                ID_Document_Ref = candidate.ID_Document_Ref
            };
        }

        public async Task<CandidateProfileDto> GetCandidateProfileByIdAsync(int candidateId)
        {
            var candidate = await _unitOfWork.Candidates.GetByIdAsync(candidateId);
            return MapToDto(candidate);
        }

        public async Task<CandidateProfileDto> GetCandidateProfileByUserIdAsync(string userId)
        {
            var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
            return MapToDto(candidate);
        }

        public async Task<CandidateProfileDto> UpdateCandidateProfileAsync(int candidateId, UpdateCandidateProfileDto dto)
        {
            var candidate = await _unitOfWork.Candidates.GetByIdAsync(candidateId);

            if (candidate == null)
            {
                return null;
            }
            
            // Update fields that can be modified after initial registration
            candidate.Email = dto.Email;
            candidate.ContactNumber = dto.ContactNumber;
            candidate.PhysicalAddress = dto.PhysicalAddress;
            candidate.PostalCode = dto.PostalCode;
            candidate.Province = dto.Province;
            candidate.HighestQualification = dto.HighestQualification;
            candidate.InstitutionName = dto.InstitutionName;
            candidate.QualificationYear = dto.QualificationYear;
            candidate.EmploymentStatus = dto.EmploymentStatus;
            candidate.OFO_Code = dto.OFO_Code;
            candidate.ID_Document_Ref = dto.ID_Document_Ref;
            candidate.AcceptsPOPI = dto.AcceptsPOPI; // Important consent field

            _unitOfWork.Candidates.Update(candidate);
            await _unitOfWork.CompleteAsync();

            return MapToDto(candidate);
        }
        
        public async Task<int?> GetCandidateIdByUserIdAsync(string userId)
        {
            var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
            return candidate?.CandidateID;
        }
    }
}