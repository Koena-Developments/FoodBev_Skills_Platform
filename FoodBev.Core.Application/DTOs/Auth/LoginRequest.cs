namespace FoodBev.Core.Application.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for user login request.
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}