using System;
using System.ComponentModel.DataAnnotations;
using FoodBev.Core.Domain.Enums;

namespace FoodBev.Application.DTOs.AdminManagement
{
    // --- OFO Code Management DTOs ---

    /// <summary>
    /// DTO for creating a new OFO Code entry.
    /// </summary>
    public class CreateOfoCodeDto
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Sector { get; set; } // e.g., "Food and Beverage Manufacturing"
    }

    /// <summary>
    /// DTO for viewing OFO Code details.
    /// </summary>
    public class OfoCodeDto : CreateOfoCodeDto
    {
        public int OfoCodeID { get; set; }
    }

    // --- Application Vetting DTOs ---

    /// <summary>
    /// DTO for the SETA Administrator to review a submitted application.
    /// </summary>
    public class ApplicationReviewDto
    {
        public int ApplicationID { get; set; }
        public int CandidateID { get; set; }
        public int EmployerID { get; set; }
        public int? SkillsProgrammeID { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        
        // Document Reference (The actual form uploaded by the employer)
        public string EnrollmentFormDocumentRef { get; set; } 

        // Additional data needed for review (e.g., linked entities)
        public string CandidateFullName { get; set; }
        public string EmployerCompanyName { get; set; }
        public string SkillsProgrammeTitle { get; set; }
    }

    /// <summary>
    /// DTO for updating the status of a Skills Programme Application by the SETA Admin.
    /// </summary>
    public class UpdateApplicationStatusDto
    {
        [Required]
        public ApplicationStatus NewStatus { get; set; }

        [StringLength(1000)]
        public string AdminNotes { get; set; }
        
        // Fields for SETA registration completion (only required for 'Registered' status)
        public string RegistrationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegisteredBy { get; set; } // Name/ID of the admin processing it
    }

    // --- Dashboard DTOs ---

    public class DashboardStatsDto
    {
        public int TotalStudents { get; set; }
        public int TotalApplications { get; set; }
        public int ActiveStudents24h { get; set; }
        public int FundedCompanies { get; set; }
    }

    public class DemographicsDto
    {
        public string Province { get; set; }
        public int Count { get; set; }
    }

    public class ActivityDto
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? ApplicationId { get; set; }
        public int? CandidateId { get; set; }
        public int? JobId { get; set; }
    }
}