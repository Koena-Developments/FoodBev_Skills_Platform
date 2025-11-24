using FoodBev.Core.Domain.Entities;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for User entity, including login and account-specific lookups.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email, int excludeUserId = 0);
        
        // Add specific methods for authentication/authorization checks here if needed
    }
}