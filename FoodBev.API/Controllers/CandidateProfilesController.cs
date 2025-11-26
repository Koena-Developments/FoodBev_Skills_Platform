using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Authorize] // Ensure only authenticated users can access these endpoints
    [ApiController]
    [Route("api/v1/candidate/profile")]
    public class CandidateProfilesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        
        // Note: Using Mediator pattern is ideal here, but using the service directly for simplicity.
        // If you were using MediatR, you'd inject IMediator and send a GetCandidateProfileQuery.

        public CandidateProfilesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        /// <summary>
        /// Retrieves the profile details for the currently authenticated candidate.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(CandidateProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfile()
        {
            // Get the authenticated User ID (must match how you store it during registration/login)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            var profile = await _candidateService.GetCandidateProfileByUserIdAsync(userId);

            if (profile == null)
            {
                return NotFound("Candidate profile not found.");
            }

            return Ok(profile);
        }

        /// <summary>
        /// Updates the profile details for the currently authenticated candidate.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(CandidateProfileDto), 200)]
        [ProducesResponseType(400)] // Bad Request for validation errors
        [ProducesResponseType(401)] // Unauthorized
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCandidateProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                // Return detailed validation errors
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { 
                    type = "ValidationError",
                    title = "One or more validation errors occurred.",
                    errors = errors 
                });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }

            // Get candidate ID from user ID
            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
            {
                return NotFound("Candidate profile not found.");
            }

            var result = await _candidateService.UpdateCandidateProfileAsync(candidateId.Value, model);

            if (result == null)
            {
                return NotFound("Candidate profile not found.");
            }
            
            return Ok(result);
        }
    }
}