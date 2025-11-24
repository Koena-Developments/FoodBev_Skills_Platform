using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Services
{
    /// <summary>
    /// Implements security operations including password hashing and JWT token generation.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;

        public SecurityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a cryptographic hash of the plain-text password using BCrypt.
        /// </summary>
        public string HashPassword(string password)
        {
            // Using PBKDF2 with SHA256 for password hashing
            const int iterations = 100000;
            const int saltSize = 16;
            const int hashSize = 32;

            // Generate a random salt
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[saltSize];
                rng.GetBytes(salt);

                // Hash the password with the salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    var hash = pbkdf2.GetBytes(hashSize);

                    // Combine salt and hash
                    var hashBytes = new byte[saltSize + hashSize];
                    Array.Copy(salt, 0, hashBytes, 0, saltSize);
                    Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

                    // Convert to base64 string for storage
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        /// <summary>
        /// Verifies a plain-text password against a stored hash.
        /// </summary>
        public bool VerifyPasswordHash(string password, string storedHash)
        {
            try
            {
                const int iterations = 100000;
                const int saltSize = 16;
                const int hashSize = 32;

                // Decode the stored hash
                var hashBytes = Convert.FromBase64String(storedHash);

                // Extract the salt
                var salt = new byte[saltSize];
                Array.Copy(hashBytes, 0, salt, 0, saltSize);

                // Compute the hash of the provided password
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    var hash = pbkdf2.GetBytes(hashSize);

                    // Compare the hashes
                    for (int i = 0; i < hashSize; i++)
                    {
                        if (hashBytes[i + saltSize] != hash[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the authenticated user.
        /// </summary>
        public Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForHS256";
            var issuer = jwtSettings["Issuer"] ?? "FoodBevAPI";
            var audience = jwtSettings["Audience"] ?? "FoodBevClient";
            var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}

