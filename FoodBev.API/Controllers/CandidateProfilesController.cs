using FoodBev.Application.DTOs.ProfileManagement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/v1/candidate/profile")]
    public class CandidateProfilesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        
    
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
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCandidateProfileDto model)
        {
            if (!ModelState.IsValid)
            {
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

            try
            {
                var result = await _candidateService.UpdateCandidateProfileAsync(candidateId.Value, model);

                if (result == null)
                {
                    return NotFound("Candidate profile not found.");
                }
                
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { 
                    type = "ValidationError",
                    title = "Update failed",
                    message = ex.Message 
                });
            }
        }
    }
}