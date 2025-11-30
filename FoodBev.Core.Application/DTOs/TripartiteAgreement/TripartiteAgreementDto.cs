using FoodBev.Core.Domain.Enums;
using System;

namespace FoodBev.Application.DTOs.TripartiteAgreement
{
    /// <summary>
    /// DTO for displaying Tripartite Agreement information.
    /// </summary>
    public class TripartiteAgreementDto
    {
        public int AgreementID { get; set; }
        public int ApplicationID { get; set; }
        public TripartiteAgreementStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Signature Status
        public bool HasCandidateSignature { get; set; }
        public DateTime? CandidateSignedDate { get; set; }
        public string? CandidateSignature { get; set; } // Base64 signature for admin view
        
        public bool HasEmployerSignature { get; set; }
        public DateTime? EmployerSignedDate { get; set; }
        public string? EmployerSignature { get; set; } // Base64 signature for admin view
        
        public bool HasTrainingProviderSignature { get; set; }
        public DateTime? TrainingProviderSignatureUploadDate { get; set; }
        public string? TrainingProviderSignatureFileRef { get; set; } // File path for admin view
        
        // Admin Review
        public DateTime? SubmittedToAdminDate { get; set; }
        public DateTime? AdminReviewedDate { get; set; }
        public string? AdminNotes { get; set; }
        
        // Related Information
        public int JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CandidateID { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public int EmployerID { get; set; }
        public string EmployerCompanyName { get; set; } = string.Empty;
    }
}

