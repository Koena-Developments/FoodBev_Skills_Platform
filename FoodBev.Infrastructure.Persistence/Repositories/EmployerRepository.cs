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

  public async Task<EmployerEntity> GetByUserIdAsync(string userId)
{
    return await _context.EmployerDetails
        .FirstOrDefaultAsync(e => e.UserID == userId);
}
    }
}