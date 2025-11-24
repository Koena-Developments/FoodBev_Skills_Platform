using FoodBev.Core.Domain.Enums;
using System;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Represents the base user account for authentication and authorization.
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        
        // Credentials
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Hashed password
        
        // Authorization
        public UserType UserType { get; set; }
        
        // Metadata
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}