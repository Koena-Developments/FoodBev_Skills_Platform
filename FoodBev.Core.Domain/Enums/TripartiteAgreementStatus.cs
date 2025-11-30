namespace FoodBev.Core.Domain.Enums
{
    /// <summary>
    /// Defines the current status of a Tripartite Agreement in the signing workflow.
    /// </summary>
    public enum TripartiteAgreementStatus
    {
        PendingCandidateSignature,    // Agreement created, waiting for candidate to sign
        AwaitingEmployerSignature,    // Candidate signed, waiting for employer to sign and upload TP signature
        SubmittedToAdmin,              // All signatures complete, awaiting admin review
        Approved,                      // Admin approved the agreement
        Rejected,                      // Admin rejected the agreement
        Archived                      // Agreement archived
    }
}

