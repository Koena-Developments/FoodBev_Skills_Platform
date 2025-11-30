using FoodBev.Core.Domain.Enums;
using System;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Represents a Tripartite Agreement that must be signed by Candidate, Employer, and Training Provider.
    /// </summary>
    public class TripartiteAgreement
    {
        public int AgreementID { get; set; }
        
        // Foreign Key to Application
        public int ApplicationID { get; set; }
        
        // Agreement Details
        public TripartiteAgreementStatus Status { get; set; } = TripartiteAgreementStatus.PendingCandidateSignature;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Candidate Signature (Digital - Base64 PNG)
        public string? CandidateSignature { get; set; }
        public DateTime? CandidateSignedDate { get; set; }
        public int? CandidateSignedByUserID { get; set; }
        
        // Employer Signature (Digital - Base64 PNG)
        public string? EmployerSignature { get; set; }
        public DateTime? EmployerSignedDate { get; set; }
        public int? EmployerSignedByUserID { get; set; }
        
        // Training Provider Signature (Uploaded File - PDF/JPG/PNG)
        public string? TrainingProviderSignatureFileRef { get; set; } // Path to uploaded file
        public DateTime? TrainingProviderSignatureUploadDate { get; set; }
        public int? TrainingProviderSignatureUploadedByUserID { get; set; } // Employer who uploaded it
        
        // Admin Review
        public DateTime? SubmittedToAdminDate { get; set; }
        public DateTime? AdminReviewedDate { get; set; }
        public int? AdminReviewedByUserID { get; set; }
        public string? AdminNotes { get; set; }
        
        // Navigation Properties
        public ApplicationEntity Application { get; set; } = null!;
        
        // Derived Properties
        public bool IsComplete => 
            !string.IsNullOrEmpty(CandidateSignature) && 
            !string.IsNullOrEmpty(EmployerSignature) && 
            !string.IsNullOrEmpty(TrainingProviderSignatureFileRef);
    }
}

