using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of IEmployerRepository.
    /// </summary>
    public class EmployerRepository : GenericRepository<EmployerEntity>, IEmployerRepository
    {
        public EmployerRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves an employer entity based on their unique Skills Development Levy (SDL) number.
        /// </summary>
        public async Task<EmployerEntity> GetByLevyNumberAsync(string levyNumber)
        {
            return await _context.EmployerDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.LevyNumber == levyNumber);
        }

        /// <summary>
        /// Retrieves an employer entity based on their user ID.
        /// </summary>
        public async Task<EmployerEntity> GetByUserIdAsync(string userId)
        {
            // Try to parse userId as int to match EmployerID
            if (int.TryParse(userId, out int employerId))
            {
                // First try by UserID field (for new records)
                var employer = await _context.EmployerDetails
                    .FirstOrDefaultAsync(e => e.UserID == userId);
                
                if (employer != null)
                    return employer;
                
                // Fallback: try by EmployerID (for existing records without UserID set)
                return await _context.EmployerDetails
                    .FirstOrDefaultAsync(e => e.EmployerID == employerId);
            }
            
            // If userId is not a valid int, try string match on UserID
            return await _context.EmployerDetails
                .FirstOrDefaultAsync(e => e.UserID == userId);
        }
    }
}