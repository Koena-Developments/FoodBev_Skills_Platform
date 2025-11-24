using System.ComponentModel.DataAnnotations;

namespace FoodBev.Application.DTOs.ProfileManagement
{
    /// <summary>
    /// DTO for retrieving and viewing an Employer's profile details.
    /// </summary>
    public class EmployerProfileDto
    {
        public int EmployerID { get; set; }
        public string UserID { get; set; } // External Auth User ID

        // Company Details
        public string CompanyName { get; set; }
        public string LevyNumber { get; set; } // Company's Levy Number (Crucial for SETA compliance)
        public string LNumber { get; set; }
        public string TNumber { get; set; }

        // Skills Development Facilitator (SDF) Details
        public string SDFName { get; set; }
        public string SDFContactNumber { get; set; }
        public string SDFEmail { get; set; }
    }

    /// <summary>
    /// DTO for updating an Employer's profile details.
    /// </summary>
    public class UpdateEmployerProfileDto
    {
        [Required]
        public string CompanyName { get; set; }
        
        // Levy Numbers
        public string LevyNumber { get; set; }
        public string LNumber { get; set; }
        public string TNumber { get; set; }

        // SDF Details
        public string SDFName { get; set; }
        [Phone]
        public string SDFContactNumber { get; set; }
        [EmailAddress]
        public string SDFEmail { get; set; }
    }
}