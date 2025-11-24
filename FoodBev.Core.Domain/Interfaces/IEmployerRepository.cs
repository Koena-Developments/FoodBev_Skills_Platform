using FoodBev.Core.Domain.Entities;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for EmployerDetail entity.
    /// </summary>
    public interface IEmployerRepository : IGenericRepository<EmployerEntity>
    {
        Task<EmployerEntity> GetByLevyNumberAsync(string levyNumber);
        Task<EmployerEntity> GetByUserIdAsync(string userId);
    }
}