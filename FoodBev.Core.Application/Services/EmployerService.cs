using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for managing Employer profiles and company details.
    /// </summary>
    public class EmployerService : IEmployerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        private EmployerProfileDto MapToDto(EmployerEntity employer)
        {
            if (employer == null) return null;

            return new EmployerProfileDto
            {
                EmployerID = employer.EmployerID,
                UserID = employer.UserID,
                
                CompanyName = employer.CompanyName,
                LevyNumber = employer.LevyNumber,
                LNumber = employer.LNumber,
                TNumber = employer.TNumber,
                
                SDFName = employer.SDFName,
                SDFContactNumber = employer.SDFContactNumber,
                SDFEmail = employer.SDFEmail
            };
        }

        public async Task<EmployerProfileDto> GetEmployerProfileByIdAsync(int employerId)
        {
            var employer = await _unitOfWork.Employers.GetByIdAsync(employerId);
            return MapToDto(employer);
        }

        public async Task<EmployerProfileDto> GetEmployerProfileByUserIdAsync(string userId)
        {
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            return MapToDto(employer);
        }

        public async Task<EmployerProfileDto> UpdateEmployerProfileAsync(int employerId, UpdateEmployerProfileDto dto)
        {
            var employer = await _unitOfWork.Employers.GetByIdAsync(employerId);

            if (employer == null)
            {
                return null;
            }
            
            // Update fields
            employer.CompanyName = dto.CompanyName;
            employer.LevyNumber = dto.LevyNumber;
            employer.LNumber = dto.LNumber;
            employer.TNumber = dto.TNumber;
            employer.SDFName = dto.SDFName;
            employer.SDFContactNumber = dto.SDFContactNumber;
            employer.SDFEmail = dto.SDFEmail;

            _unitOfWork.Employers.Update(employer);
            await _unitOfWork.CompleteAsync();

            return MapToDto(employer);
        }

        public async Task<int?> GetEmployerIdByUserIdAsync(string userId)
        {
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            return employer?.EmployerID;
        }

        public async Task<EmployerProfileDto> GetEmployerProfileAsync(string userId)
        {
            return await GetEmployerProfileByUserIdAsync(userId);
        }

        public async Task<bool> UpdateEmployerProfileAsync(string userId, UpdateEmployerProfileDto dto)
        {
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return false;
            }

            var result = await UpdateEmployerProfileAsync(employer.EmployerID, dto);
            return result != null;
        }
    }
}