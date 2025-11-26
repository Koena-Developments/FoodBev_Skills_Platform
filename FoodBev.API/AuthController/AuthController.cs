using FoodBev.Application.DTOs.Authentication;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodBev.API.Controllers
{
    /// <summary>
    /// Handles user authentication endpoints: registration, login, and token validation.
    /// </summary>
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user account.
        /// POST /api/Auth/register
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", registrationDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registrationDto);

            if (!result.IsAuthenticated)
            {
                _logger.LogWarning("Registration failed for user {Email}. Error: {Error}", registrationDto.Email, result.Message);
                return BadRequest(new { Errors = new[] { result.Message } });
            }

            _logger.LogInformation("User {Email} registered successfully.", registrationDto.Email);
            return Ok(result);
        }

        /// <summary>
        /// Logs in an existing user.
        /// POST /api/Auth/login
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Attempting login for user with email: {Email}", loginDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
            {
                _logger.LogWarning("Login failed for user {Email}. Error: {Error}", loginDto.Email, result.Message);
                return Unauthorized(new { Errors = new[] { result.Message } });
            }

            _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
            return Ok(result);
        }

        /// <summary>
        /// Validates a JWT token to confirm user identity.
        /// POST /api/Auth/validateToken
        /// </summary>
        [HttpPost("validateToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ValidateToken([FromHeader(Name = "Authorization")] string? authorization)
        {
            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized(new { Errors = new[] { "Missing or invalid Authorization header." } });
            }

            var token = authorization["Bearer ".Length..].Trim();
            _logger.LogInformation("Attempting to validate token.");

            var result = await _authService.ValidateTokenAsync(token);

            if (!result.IsAuthenticated)
            {
                _logger.LogWarning("Token validation failed.");
                return Unauthorized(new { Errors = result.Message });
            }

            _logger.LogInformation("Token validated successfully for user: {Email}", result.Email);
            return Ok(result);
        }
    }
}