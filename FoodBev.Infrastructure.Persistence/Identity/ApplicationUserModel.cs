using Microsoft.AspNetCore.Identity;

namespace FoodBev.Infrastructure.Persistence.Identity
{
    /// <summary>
    /// Custom Identity user class to extend the default fields with domain-specific properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Custom properties for user tracking or management can be added here, e.g.:
        // public DateTime DateCreated { get; set; }
        // public bool IsActive { get; set; }
    }
}