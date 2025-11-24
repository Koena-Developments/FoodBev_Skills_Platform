using FoodBev.Application.DTOs.JobManagement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [ApiController]
    [Route("api/v1/jobs")]
    public class JobPostingsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobPostingsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        /// <summary>
        /// Searches for jobs based on various criteria. Accessible to all.
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchJobs([FromQuery] JobSearchDto search)
        {
            var jobs = await _jobService.SearchJobsAsync(search);
            // In a real app, you would return a PaginatedList<JobPostingDto>
            return Ok(jobs);
        }

        /// <summary>
        /// Retrieves a single job posting by ID. Accessible to all.
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJob(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        // --- Employer-only Actions ---

        /// <summary>
        /// Creates a new job posting. Requires Employer role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobPostingDto model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var jobId = await _jobService.CreateJobAsync(userId, model);

            return CreatedAtAction(nameof(GetJob), new { id = jobId }, new { JobId = jobId });
        }

        /// <summary>
        /// Updates an existing job posting. Requires Employer role.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobPostingDto model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await _jobService.UpdateJobAsync(userId, id, model);

            if (!success)
            {
                return BadRequest("Job update failed. Ensure the job exists and you are the owner.");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a job posting. Requires Employer role.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await _jobService.DeleteJobAsync(userId, id);

            if (!success)
            {
                return NotFound("Job not found or unauthorized to delete.");
            }

            return NoContent();
        }
    }
}