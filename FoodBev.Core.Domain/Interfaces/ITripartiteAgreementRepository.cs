using FoodBev.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for Tripartite Agreement operations.
    /// </summary>
    public interface ITripartiteAgreementRepository : IGenericRepository<TripartiteAgreement>
    {
        /// <summary>
        /// Gets an agreement by application ID.
        /// </summary>
        Task<TripartiteAgreement?> GetByApplicationIdAsync(int applicationId);

        /// <summary>
        /// Gets all agreements for a candidate.
        /// </summary>
        Task<IEnumerable<TripartiteAgreement>> GetByCandidateIdAsync(int candidateId);

        /// <summary>
        /// Gets all agreements for an employer.
        /// </summary>
        Task<IEnumerable<TripartiteAgreement>> GetByEmployerIdAsync(int employerId);

        /// <summary>
        /// Gets all agreements pending admin review.
        /// </summary>
        Task<IEnumerable<TripartiteAgreement>> GetPendingAdminReviewAsync();
    }
}

