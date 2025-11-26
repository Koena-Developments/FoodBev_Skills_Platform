namespace FoodBev.Core.Domain.Enums
{
    /// <summary>
    /// Defines the current processing status of a job application.
    /// </summary>
    public enum ApplicationStatus
    {
        Applied,        // Initial submission by candidate (renamed from Submitted)
        Saved,          // Draft application
        Shortlisted,    // Candidate is being considered
        InterviewScheduled,  // Interview has been scheduled by employer
        InterviewAccepted,  // Candidate accepted the interview
        InterviewDeclined,  // Candidate declined the interview
        Rejected,       // Application was rejected
        Hired,          // Candidate was successful and hired
        Registered,     // Candidate was registered with the SETA
    }
}