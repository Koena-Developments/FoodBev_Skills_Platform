using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Authorize] // Ensure only authenticated users can access these endpoints
    [ApiController]
    [Route("api/v1/employer/profile")]
    public class EmployerProfilesController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployerProfilesController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        private bool IsEmployer()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return role == "Employer";
        }

        /// <summary>
        /// Retrieves the profile details for the currently authenticated employer (company profile).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(EmployerProfileDto), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            // Check if user is an Employer
            if (!IsEmployer())
            {
                return StatusCode(403, new { message = "Only employers can access this endpoint." });
            }

            var profile = await _employerService.GetEmployerProfileAsync(userId);

            if (profile == null)
            {
                return NotFound("Employer profile not found. Please complete your company setup.");
            }

            return Ok(profile);
        }

        /// <summary>
        /// Updates the profile details for the currently authenticated employer.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateEmployerProfileDto model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            // Check if user is an Employer
            if (!IsEmployer())
            {
                return StatusCode(403, new { message = "Only employers can update their profile." });
            }

            var result = await _employerService.UpdateEmployerProfileAsync(userId, model);

            if (result)
            {
                return NoContent(); // Success
            }

            return BadRequest("Could not update company profile. Validation or business logic failed.");
        }
    }
}