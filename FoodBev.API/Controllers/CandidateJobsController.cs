using FoodBev.Application.DTOs.JobManagement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [ApiController]
    [Route("api/v1/candidate/jobs")]
    [Authorize] // Ensure only authenticated users can access these endpoints
    public class CandidateJobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public CandidateJobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        private bool IsCandidate()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return role == "Candidate";
        }

        /// <summary>
        /// Gets jobs matching the candidate's profile (OFO code based).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetMatchingJobs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if user is a Candidate
            if (!IsCandidate())
            {
                return StatusCode(403, new { message = "Only candidates can access this endpoint." });
            }

            var jobs = await _jobService.GetMatchingJobsForCandidateByUserIdAsync(userId);
            return Ok(jobs);
        }
    }
}

