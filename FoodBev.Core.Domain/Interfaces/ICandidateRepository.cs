using FoodBev.Core.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for CandidateDetail entity.
    /// </summary>
    public interface ICandidateRepository : IGenericRepository<CandidateEntity>
    {
        Task<CandidateEntity> GetByUserIdAsync(string userId);
        Task<CandidateEntity> GetCandidateBySAIdAsync(string saIdNumber);
        
        // Custom query to find candidates matching specific job criteria (e.g., OFO code)
        Task<IEnumerable<CandidateEntity>> GetMatchingCandidatesAsync(string requiredOfoCode);
    }
}