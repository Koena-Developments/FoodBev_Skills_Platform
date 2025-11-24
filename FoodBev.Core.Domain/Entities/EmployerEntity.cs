namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Stores company and SDF information for an employer.
    /// Has a one-to-one relationship with the User entity (EmployerID is PK and FK).
    /// </summary>
    public class EmployerEntity
    {
        // PK and FK to User.UserID. Mapped as a one-to-one relationship in DbContext.
        public int EmployerID { get; set; }
        public string? UserID { get; set; } 
        public string? LNumber { get; set; } 
        public string? TNumber { get; set; }
        
        public string? CompanyName { get; set; }
        public string? LevyNumber { get; set; } // Skills Development Levy number
        
        // Skills Development Facilitator (SDF) Details
        public string? SDFName { get; set; }
        public string? SDFEmail { get; set; }
        public string? SDFContactNumber { get; set; }
    }
}