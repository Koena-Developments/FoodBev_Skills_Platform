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
    [Authorize(Roles = "Candidate")]
    public class CandidateJobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public CandidateJobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// Gets jobs matching the candidate's profile (OFO code based).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMatchingJobs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var jobs = await _jobService.GetMatchingJobsForCandidateByUserIdAsync(userId);
            return Ok(jobs);
        }
    }
}

