using FoodBev.Application.DTOs.Authentication;
using FoodBev.Core.Domain.Enums;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for user authentication, registration, and authorization services.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user based on the provided DTO.
        /// </summary>
        Task<AuthResponseDto> RegisterAsync(UserRegistrationDto registrationDto);

        /// <summary>
        /// Authenticates a user based on email and password.
        /// </summary>
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Validates a user's token and retrieves their information.
        /// </summary>
        Task<AuthResponseDto> ValidateTokenAsync(string token);

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }
}