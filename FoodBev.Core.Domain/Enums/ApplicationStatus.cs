namespace FoodBev.Core.Domain.Enums
{
    /// <summary>
    /// Defines the current processing status of a job application.
    /// </summary>
    public enum ApplicationStatus
    {
        Submitted,      // Initial submission by candidate
        Saved,          // Draft application
        Rejected,       // Application was rejected
        Shortlisted,    // Candidate is being considered
        Interview_Now,  // Candidate is scheduled for interview
        Hired,          // Candidate was successful and hired
        Registered,     // Candidate was registered with the SETA

    }
}