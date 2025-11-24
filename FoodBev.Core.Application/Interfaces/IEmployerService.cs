using FoodBev.Application.DTOs.ProfileManagement;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing Employer profiles and company details.
    /// </summary>
    public interface IEmployerService
    {
        /// <summary>
        /// Retrieves an Employer's profile by their internal ID.
        /// </summary>
        Task<EmployerProfileDto> GetEmployerProfileByIdAsync(int employerId);

        /// <summary>
        /// Retrieves an Employer's profile by their external UserID (from the Auth service).
        /// </summary>
        Task<EmployerProfileDto> GetEmployerProfileByUserIdAsync(string userId);

        /// <summary>
        /// Updates the company and SDF details of an Employer's profile.
        /// </summary>
        Task<EmployerProfileDto> UpdateEmployerProfileAsync(int employerId, UpdateEmployerProfileDto dto);

        /// <summary>
        /// Retrieves the Employer ID linked to an external UserID.
        /// </summary>
        Task<int?> GetEmployerIdByUserIdAsync(string userId);

        /// <summary>
        /// Retrieves an Employer's profile by their external UserID (convenience method).
        /// </summary>
        Task<EmployerProfileDto> GetEmployerProfileAsync(string userId);

        /// <summary>
        /// Updates an Employer's profile by their external UserID.
        /// </summary>
        Task<bool> UpdateEmployerProfileAsync(string userId, UpdateEmployerProfileDto dto);
    }
}