using FoodBev.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Represents a candidate's formal application to a job posting.
    /// </summary>
    public class ApplicationEntity
    {
        // Primary key - configured via EF Core fluent API in ApplicationDbContext
        public int ApplicationID { get; set; }
        
        // Foreign Keys
        public int JobID { get; set; }
        public int CandidateID { get; set; }
        
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
        
        // Interview Details
        public DateTime? InterviewDate { get; set; }
        public string InterviewVenue { get; set; } = string.Empty;
        public string CandidateAvailability { get; set; } = string.Empty;
        
        public string CV_File_Ref { get; set; } = string.Empty;


        // Admin & Compliance Fields
        public string? AdminNotes { get; set; }
        public string? RegistrationNumber { get; set; }
        public DateTime? DateOfRegistration { get; set; }
        public string? RegisteredBy { get; set; }
        public string? EnrollmentFormDocumentRef { get; set; }

        // Optional: If you need direct access to SkillsProgramme (not just the form)
        // public int? SkillsProgrammeID { get; set; }

        // Navigation Properties
        public JobPosting Job { get; set; } = null!;
        public CandidateEntity Candidate { get; set; } = null!;
        public SkillsProgrammeForm? SkillsProgrammeForm { get; set; }

        // Derive EmployerID from Job (no need to store redundantly)
        public int EmployerID => Job?.EmployerID ?? 0;

        // SubmissionDate = DateApplied (no need for separate field)
        public DateTime SubmissionDate => DateApplied;
    }
}