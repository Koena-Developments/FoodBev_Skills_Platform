using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of IUserRepository.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a user entity based on their unique email address.
        /// </summary>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            // FirstOrDefaultAsync ensures we don't throw if the user is not found
            return await _context.Users
                .AsNoTracking() // Read-only query for performance
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Checks if an email address is unique (not already in use by another user).
        /// Returns true if the email is unique, false if it already exists.
        /// </summary>
        public async Task<bool> IsEmailUniqueAsync(string email, int excludeUserId = 0)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == email && u.UserID != excludeUserId);
            return !emailExists; // Return true if email is unique (doesn't exist)
        }
    }
}