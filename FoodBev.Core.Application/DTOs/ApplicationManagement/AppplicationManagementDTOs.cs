using FoodBev.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoodBev.Application.DTOs.ApplicationManagement
{
    /// <summary>
    /// DTO for a candidate to submit a new job application.
    /// </summary>
    public class CreateApplicationDto
    {
        [Required]
        public int JobID { get; set; }
        
        [Required]
        public int CandidateID { get; set; }

        [Required]
        public string CV_File_Ref { get; set; } // Reference to the uploaded CV file

        // Optional preliminary details
        public string CandidateAvailability { get; set; }
    }

    /// <summary>
    /// DTO for reading application status and brief details.
    /// </summary>
    public class ApplicationSummaryDto
    {
        public int ApplicationID { get; set; }
        public int JobID { get; set; }
        public int CandidateID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateApplied { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string InterviewVenue { get; set; }
        public bool HasSkillsForm { get; set; }
    }
    
    /// <summary>
    /// DTO for signing the Skills Programme Form.
    /// </summary>
    public class SignatureDto
    {
        [Required]
        public int ApplicationID { get; set; }
        
        [Required]
        public string SignatureBase64 { get; set; } // Base64 representation of the digital signature
    }

    /// <summary>
    /// DTO for viewing the Skills Programme Form details.
    /// </summary>
    public class SkillsFormDto
    {
        public int FormID { get; set; }
        public int ApplicationID { get; set; }
        public FormStatus Status { get; set; }
        public string CandidateSignatureStatus { get; set; }
        public string EmployerSignatureStatus { get; set; }
        public string AdminRegistrationNo { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}