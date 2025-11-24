using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/employer/jobs")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerJobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IEmployerService _employerService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public EmployerJobsController(
            IJobService jobService,
            IEmployerService employerService)
        {
            _jobService = jobService;
            _employerService = employerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            var jobs = await _jobService.GetJobsByEmployerAsync(employerId.Value);
            return Ok(jobs);
        }
    }
}