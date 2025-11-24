using System;
using System.Collections.Generic;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Stores demographic and skills information for a job candidate.
    /// Has a one-to-one relationship with the User entity.
    /// </summary>
    public class CandidateEntity
    {
        public int CandidateID { get; set; }
        
        // Personal Details
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IDNumber { get; set; } // SA ID Number
        public DateTime? DateOfBirth { get; set; }
        public string? Race { get; set; }
        public string? Gender { get; set; }
        public bool IsDisabled { get; set; }
        public string? DisabilityDetails { get; set; }
        public string? Nationality { get; set; }
        
        // Contact Details
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? PhysicalAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? Province { get; set; }
        
        // Education & Status
        public string? HighestQualification { get; set; }
        public string? InstitutionName { get; set; }
        public int? QualificationYear { get; set; }
        public string? EmploymentStatus { get; set; } // e.g., "Employed", "Unemployed"
        public string? OFO_Code { get; set; }

        // Compliance
        public bool AcceptsPOPI { get; set; } // Boolean: true = consent given

        // Documents
        public string? ID_Document_Ref { get; set; }

        // Navigation
        public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    }
}