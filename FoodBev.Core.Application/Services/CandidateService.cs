using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using System;
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
            if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out int candidateId))
            {
                return null;
            }

            // First, try to get existing candidate
            var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
            
            // If candidate profile doesn't exist, verify user exists and create profile
            if (candidate == null)
            {
                // Verify the user exists and is a candidate
                var user = await _unitOfWork.Users.GetByIdAsync(candidateId);
                if (user == null || user.UserType != UserType.Candidate)
                {
                    return null; // User doesn't exist or isn't a candidate
                }

                // User exists and is a candidate, but profile is missing - create it
                try
                {
                    candidate = new CandidateEntity 
                    { 
                        CandidateID = candidateId,
                        // Initialize all required fields to avoid constraint issues
                        AcceptsPOPI = false
                    };
                    await _unitOfWork.Candidates.AddAsync(candidate);
                    await _unitOfWork.CompleteAsync();
                    
                    // Fetch the newly created candidate
                    candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                {
                    // If creation fails (e.g., already exists), try to fetch again
                    candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
                }
            }
            
            return MapToDto(candidate);
        }

        public async Task<CandidateProfileDto> UpdateCandidateProfileAsync(int candidateId, UpdateCandidateProfileDto dto)
        {
            var candidate = await _unitOfWork.Candidates.GetByIdAsync(candidateId);

            if (candidate == null)
            {
                return null;
            }
            
            // Validate IDNumber uniqueness if it's being changed
            if (!string.IsNullOrWhiteSpace(dto.IDNumber) && dto.IDNumber != candidate.IDNumber)
            {
                var existingCandidate = await _unitOfWork.Candidates.GetCandidateBySAIdAsync(dto.IDNumber);
                if (existingCandidate != null && existingCandidate.CandidateID != candidateId)
                {
                    throw new InvalidOperationException($"ID Number '{dto.IDNumber}' is already registered to another candidate.");
                }
            }
            
            // Update all fields (only update if provided)
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                candidate.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName))
                candidate.LastName = dto.LastName;
            if (!string.IsNullOrWhiteSpace(dto.IDNumber))
                candidate.IDNumber = dto.IDNumber;
            if (dto.DateOfBirth.HasValue)
                candidate.DateOfBirth = dto.DateOfBirth;
            if (!string.IsNullOrWhiteSpace(dto.Gender))
                candidate.Gender = dto.Gender;
            if (!string.IsNullOrWhiteSpace(dto.Race))
                candidate.Race = dto.Race;
            if (!string.IsNullOrWhiteSpace(dto.Nationality))
                candidate.Nationality = dto.Nationality;
            if (dto.IsDisabled.HasValue)
                candidate.IsDisabled = dto.IsDisabled.Value;
            if (!string.IsNullOrWhiteSpace(dto.DisabilityDetails))
                candidate.DisabilityDetails = dto.DisabilityDetails;
            
            // Update contact details
            if (!string.IsNullOrWhiteSpace(dto.Email))
                candidate.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.ContactNumber))
                candidate.ContactNumber = dto.ContactNumber;
            if (!string.IsNullOrWhiteSpace(dto.PhysicalAddress))
                candidate.PhysicalAddress = dto.PhysicalAddress;
            if (!string.IsNullOrWhiteSpace(dto.PostalCode))
                candidate.PostalCode = dto.PostalCode;
            if (!string.IsNullOrWhiteSpace(dto.Province))
                candidate.Province = dto.Province;
            
            // Update education & employment
            if (!string.IsNullOrWhiteSpace(dto.HighestQualification))
                candidate.HighestQualification = dto.HighestQualification;
            if (!string.IsNullOrWhiteSpace(dto.InstitutionName))
                candidate.InstitutionName = dto.InstitutionName;
            if (dto.QualificationYear.HasValue)
                candidate.QualificationYear = dto.QualificationYear;
            if (!string.IsNullOrWhiteSpace(dto.EmploymentStatus))
                candidate.EmploymentStatus = dto.EmploymentStatus;
            if (!string.IsNullOrWhiteSpace(dto.OFO_Code))
                candidate.OFO_Code = dto.OFO_Code;
            
            // Update documents and consent
            if (!string.IsNullOrWhiteSpace(dto.ID_Document_Ref))
                candidate.ID_Document_Ref = dto.ID_Document_Ref;
            candidate.AcceptsPOPI = dto.AcceptsPOPI; // Important consent field

            _unitOfWork.Candidates.Update(candidate);
            await _unitOfWork.CompleteAsync();

            return MapToDto(candidate);
        }
        
        public async Task<int?> GetCandidateIdByUserIdAsync(string userId)
        {
            var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
            
            // If candidate profile doesn't exist but user exists, create an empty profile
            if (candidate == null && int.TryParse(userId, out int candidateId))
            {
                // Verify the user exists and is a candidate
                var user = await _unitOfWork.Users.GetByIdAsync(candidateId);
                if (user != null && user.UserType == UserType.Candidate)
                {
                    try
                    {
                        // Create an empty candidate profile with all required fields initialized
                        candidate = new CandidateEntity 
                        { 
                            CandidateID = candidateId,
                            AcceptsPOPI = false // Initialize required field to avoid constraint issues
                        };
                        await _unitOfWork.Candidates.AddAsync(candidate);
                        await _unitOfWork.CompleteAsync();
                        
                        // Re-fetch to ensure it was created successfully
                        candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
                    }
                    catch
                    {
                        // If creation fails, try to fetch again in case it was created by another process
                        candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
                    }
                }
            }
            
            return candidate?.CandidateID;
        }
    }
}