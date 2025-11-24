namespace FoodBev.Core.Application.DTOs.Auth
{
    /// <summary>
    /// Data Transfer Object for user registration request.
    /// </summary>
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// The desired role for the user (e.g., "Admin", "User", etc.). Defaults to "User".
        /// </summary>
        public string Role { get; set; } = "User"; 
    }
}