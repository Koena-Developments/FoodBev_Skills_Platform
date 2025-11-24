using FoodBev.Core.Domain.Entities;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for security-related operations, such as password hashing and JWT generation.
    /// This keeps core business logic decoupled from security implementation details.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Creates a cryptographic hash of the plain-text password.
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plain-text password against a stored hash.
        /// </summary>
        bool VerifyPasswordHash(string password, string storedHash);

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the authenticated user.
        /// </summary>
        Task<string> GenerateJwtTokenAsync(User user);
    }
}