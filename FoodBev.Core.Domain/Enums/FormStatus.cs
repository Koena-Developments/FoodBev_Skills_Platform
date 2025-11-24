namespace FoodBev.Core.Domain.Enums
{
    /// <summary>
    /// Defines the current signing/completion status of the Skills Programme Form.
    /// </summary>
    public enum FormStatus
    {
        Pending_Candidate,      // Waiting for candidate signature
        Pending_Employer,       // Waiting for employer signature
        Pending_Admin,          // Waiting for FoodBev SETA admin registration
        Complete                // All parties have signed and registered
    }
}