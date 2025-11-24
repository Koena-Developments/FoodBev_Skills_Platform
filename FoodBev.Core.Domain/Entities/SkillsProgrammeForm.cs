using FoodBev.Core.Domain.Enums;
using System;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Represents the physical Skills Programme Enrolment Form, linked to an application.
    /// </summary>
    public class SkillsProgrammeForm
    {
        public int FormID { get; set; }
        
        // PK and FK to Application.ApplicationID. Mapped as a one-to-one relationship in DbContext.
        public int ApplicationID { get; set; } 
        
        public FormStatus Status { get; set; } = FormStatus.Pending_Candidate;
        
        // Digital Signatures (Stored as Base64 image strings or reference to a signature file)
        public string CandidateSignature { get; set; }
        public string EmployerSignature { get; set; }
        public string TrainingProviderSignature { get; set; }
        
        // Dates
        public DateTime DateSubmitted { get; set; }
        public DateTime? DateCompleted { get; set; }
        
        // FoodBev Admin Use Only Fields (from the back of the form)
        public string AdminRegistrationNo { get; set; }
        
        // Navigation Property
        public ApplicationEntity Application { get; set; }
    }
}