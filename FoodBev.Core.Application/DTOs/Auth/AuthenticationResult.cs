namespace FoodBev.Core.Application.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for authentication response, containing the JWT token and user details.
    /// </summary>
    public class AuthResult
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}