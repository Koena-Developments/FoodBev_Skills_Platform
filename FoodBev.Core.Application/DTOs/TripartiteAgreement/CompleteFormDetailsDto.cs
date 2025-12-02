using FoodBev.Core.Domain.Enums;
using System;

namespace FoodBev.Application.DTOs.TripartiteAgreement
{
    /// <summary>
    /// Complete form details DTO for admin view and PDF generation.
    /// Includes all candidate, employer, and agreement information.
    /// </summary>
    public class CompleteFormDetailsDto
    {
        // Agreement Information
        public int AgreementID { get; set; }
        public int ApplicationID { get; set; }
        public TripartiteAgreementStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedToAdminDate { get; set; }
        public DateTime? AdminReviewedDate { get; set; }
        public string? AdminNotes { get; set; }

        // Signatures
        public string? CandidateSignature { get; set; } // Base64
        public DateTime? CandidateSignedDate { get; set; }
        public string? EmployerSignature { get; set; } // Base64
        public DateTime? EmployerSignedDate { get; set; }
        public string? TrainingProviderSignatureFileRef { get; set; }
        public DateTime? TrainingProviderSignatureUploadDate { get; set; }

        // Job Information
        public int JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string? JobDescription { get; set; }
        public string? OFO_Code_Required { get; set; }

        // Candidate Details
        public int CandidateID { get; set; }
        public string CandidateFirstName { get; set; } = string.Empty;
        public string CandidateLastName { get; set; } = string.Empty;
        public string? CandidateIDNumber { get; set; }
        public DateTime? CandidateDateOfBirth { get; set; }
        public string? CandidateRace { get; set; }
        public string? CandidateGender { get; set; }
        public bool CandidateIsDisabled { get; set; }
        public string? CandidateDisabilityDetails { get; set; }
        public string? CandidateNationality { get; set; }
        public string? CandidateContactNumber { get; set; }
        public string? CandidateEmail { get; set; }
        public string? CandidatePhysicalAddress { get; set; }
        public string? CandidatePostalCode { get; set; }
        public string? CandidateProvince { get; set; }
        public string? CandidateHighestQualification { get; set; }
        public string? CandidateInstitutionName { get; set; }
        public int? CandidateQualificationYear { get; set; }
        public string? CandidateEmploymentStatus { get; set; }
        public string? CandidateOFO_Code { get; set; }
        public string? CandidateID_Document_Ref { get; set; }

        // Employer Details
        public int EmployerID { get; set; }
        public string EmployerCompanyName { get; set; } = string.Empty;
        public string? EmployerLevyNumber { get; set; }
        public string? EmployerLNumber { get; set; }
        public string? EmployerTNumber { get; set; }
        public string? EmployerSDFName { get; set; }
        public string? EmployerSDFEmail { get; set; }
        public string? EmployerSDFContactNumber { get; set; }

        // Application Details
        public DateTime ApplicationDateApplied { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime? ApplicationInterviewDate { get; set; }
        public string? ApplicationInterviewVenue { get; set; }
        public string? ApplicationCV_File_Ref { get; set; }
    }
}

