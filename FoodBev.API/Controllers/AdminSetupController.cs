using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FoodBev.API.Controllers
{
    /// <summary>
    /// Controller for creating the initial admin account.
    /// This is a one-time setup endpoint that should be secured or removed after first use.
    /// </summary>
    [Route("api/v1/admin/setup")]
    [ApiController]
    public class AdminSetupController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminSetupController> _logger;

        public AdminSetupController(
            IUnitOfWork unitOfWork,
            ISecurityService securityService,
            IConfiguration configuration,
            ILogger<AdminSetupController> logger)
        {
            _unitOfWork = unitOfWork;
            _securityService = securityService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Creates the initial admin account.
        /// This endpoint requires a secret key to prevent unauthorized admin creation.
        /// POST /api/v1/admin/setup/create
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAdminAccount([FromBody] AdminSetupDto dto)
        {
            // Verify secret key (you can set this in appsettings.json or environment variable)
            var requiredSecretKey = _configuration["AdminSetup:SecretKey"] ?? "CHANGE_THIS_SECRET_KEY_IN_PRODUCTION";
            
            // Trim and compare (case-sensitive for security)
            var providedKey = dto.SecretKey?.Trim() ?? string.Empty;
            var expectedKey = requiredSecretKey.Trim();
            
            _logger.LogInformation("Admin setup attempt - Expected key length: {ExpectedLength}, Provided key length: {ProvidedLength}", 
                expectedKey.Length, providedKey.Length);
            
            if (string.IsNullOrEmpty(providedKey) || providedKey != expectedKey)
            {
                _logger.LogWarning("Unauthorized attempt to create admin account. Key mismatch.");
                return Unauthorized(new { message = "Invalid secret key. Please check the secret key in appsettings.json." });
            }

            // Check if admin already exists
            var existingAdmin = await _unitOfWork.Users.GetUserByEmailAsync(dto.Email);
            if (existingAdmin != null)
            {
                return BadRequest(new { message = "An account with this email already exists." });
            }

            // Check if any admin exists
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var adminExists = allUsers.Any(u => u.UserType == UserType.Admin);
            if (adminExists)
            {
                return BadRequest(new { message = "An admin account already exists. Use the login endpoint instead." });
            }

            // Create admin user
            var adminUser = new User
            {
                Email = dto.Email,
                PasswordHash = _securityService.HashPassword(dto.Password),
                UserType = UserType.Admin,
                IsActive = true,
                RegistrationDate = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(adminUser);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Admin account created successfully for email: {Email}", dto.Email);

            return Ok(new { message = "Admin account created successfully. You can now login at /admin/login" });
        }
    }

    /// <summary>
    /// DTO for admin account creation.
    /// </summary>
    public class AdminSetupDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}

