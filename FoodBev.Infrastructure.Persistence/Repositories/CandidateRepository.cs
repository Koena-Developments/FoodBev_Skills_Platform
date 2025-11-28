using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of ICandidateRepository.
    /// </summary>
    public class CandidateRepository : GenericRepository<CandidateEntity>, ICandidateRepository
    {
        public CandidateRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a candidate entity based on their user ID.
        /// </summary>
        public async Task<CandidateEntity> GetByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out int candidateId))
            {
                return null;
            }

            return await _context.CandidateDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CandidateID == candidateId);
        }

        /// <summary>
        /// Retrieves a candidate entity based on their unique South African ID number.
        /// </summary>
        public async Task<CandidateEntity> GetCandidateBySAIdAsync(string saIdNumber)
        {
            return await _context.CandidateDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IDNumber == saIdNumber);
        }

        /// <summary>
        /// Retrieves candidates whose OFO code matches the required OFO code for a job.
        /// This is used by the Employer/Admin to shortlist relevant learners.
        /// </summary>
        public async Task<IEnumerable<CandidateEntity>> GetMatchingCandidatesAsync(string requiredOfoCode)
        {
            return await _context.CandidateDetails
                .Where(c => c.OFO_Code == requiredOfoCode)
                .ToListAsync();
        }
    }
}