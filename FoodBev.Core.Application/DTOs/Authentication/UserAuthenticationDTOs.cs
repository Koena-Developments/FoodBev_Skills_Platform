using FoodBev.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FoodBev.Application.DTOs.Authentication
{
    /// <summary>
    /// DTO for user registration data coming from the frontend.
    /// </summary>
    public class UserRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; } // Candidate or Employer
    }

    /// <summary>
    /// DTO for user login credentials.
    /// </summary>
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    /// <summary>
    /// DTO for the response after successful login or registration.
    /// </summary>
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public bool IsAuthenticated { get; set; } = true;
        public string Message { get; set; }
    }
}