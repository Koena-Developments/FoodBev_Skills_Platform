using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of IOfoCodeRepository.
    /// </summary>
    public class OfoCodeRepository : GenericRepository<OfoCode>, IOfoCodeRepository
    {
        public OfoCodeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

