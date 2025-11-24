using System;
using System.ComponentModel.DataAnnotations;

namespace FoodBev.Application.DTOs.ProfileManagement
{
    /// <summary>
    /// DTO for retrieving and viewing a Candidate's profile details.
    /// </summary>
    public class CandidateProfileDto
    {
        public int CandidateID { get; set; }
        public string UserID { get; set; } // External Auth User ID

        // Personal Details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public bool IsDisabled { get; set; }
        public string DisabilityDetails { get; set; }
        public string Nationality { get; set; }
        
        // Contact Details
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        
        // Education & Status (Crucial for Matching/Learnership)
        public string HighestQualification { get; set; }
        public string InstitutionName { get; set; }
        public int QualificationYear { get; set; }
        public string EmploymentStatus { get; set; } // e.g., "Employed", "Unemployed"
        public string OFO_Code { get; set; } // Occupational Classification Code
        public bool AcceptsPOPI { get; set; } // POPI Act Consent

        // Documents
        public string ID_Document_Ref { get; set; } // Reference to uploaded ID document
    }

    /// <summary>
    /// DTO for updating a Candidate's profile details.
    /// </summary>
    public class UpdateCandidateProfileDto
    {
        // Contact Details
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string ContactNumber { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        
        // Education & Status
        public string HighestQualification { get; set; }
        public string InstitutionName { get; set; }
        public int QualificationYear { get; set; }
        public string EmploymentStatus { get; set; }
        
        // OFO Code and Documents
        [Required]
        public string OFO_Code { get; set; }
        public string ID_Document_Ref { get; set; }
        
        // Consent
        public bool AcceptsPOPI { get; set; }
    }
}