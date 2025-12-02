using FoodBev.Application.DTOs.TripartiteAgreement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Service interface for managing Tripartite Agreements.
    /// </summary>
    public interface ITripartiteAgreementService
    {
        /// <summary>
        /// Creates a new Tripartite Agreement for an application.
        /// </summary>
        Task<TripartiteAgreementDto> CreateAgreementAsync(int applicationId);

        /// <summary>
        /// Gets an agreement by ID.
        /// </summary>
        Task<TripartiteAgreementDto?> GetAgreementByIdAsync(int agreementId);

        /// <summary>
        /// Gets an agreement by application ID.
        /// </summary>
        Task<TripartiteAgreementDto?> GetAgreementByApplicationIdAsync(int applicationId);

        /// <summary>
        /// Gets all agreements for a candidate.
        /// </summary>
        Task<IEnumerable<TripartiteAgreementDto>> GetAgreementsByCandidateIdAsync(int candidateId);

        /// <summary>
        /// Gets all agreements for an employer.
        /// </summary>
        Task<IEnumerable<TripartiteAgreementDto>> GetAgreementsByEmployerIdAsync(int employerId);

        /// <summary>
        /// Gets all agreements pending admin review.
        /// </summary>
        Task<IEnumerable<TripartiteAgreementDto>> GetPendingAdminReviewAsync();

        /// <summary>
        /// Candidate signs the agreement.
        /// </summary>
        Task<bool> SignAsCandidateAsync(int agreementId, string userId, string signatureBase64);

        /// <summary>
        /// Employer signs the agreement and uploads Training Provider signature.
        /// </summary>
        Task<bool> SignAsEmployerAsync(int agreementId, string userId, string employerSignatureBase64, string? trainingProviderSignatureFileRef);

        /// <summary>
        /// Admin reviews and approves/rejects the agreement.
        /// </summary>
        Task<bool> ReviewAgreementAsync(int agreementId, string userId, bool approved, string? notes);

        /// <summary>
        /// Gets complete form details including all candidate, employer, and agreement information for admin view and PDF generation.
        /// </summary>
        Task<CompleteFormDetailsDto?> GetCompleteFormDetailsAsync(int agreementId);
    }
}

