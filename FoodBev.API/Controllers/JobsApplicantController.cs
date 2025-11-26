using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/jobs/{jobId:int}/applicants")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class JobApplicantsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ICandidateService _candidateService;
        private readonly IJobService _jobService;
        private readonly IEmployerService _employerService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public JobApplicantsController(
            IApplicationService applicationService,
            ICandidateService candidateService,
            IJobService jobService,
            IEmployerService employerService)
        {
            _applicationService = applicationService;
            _candidateService = candidateService;
            _jobService = jobService;
            _employerService = employerService;
        }

        /// <summary>
        /// Get all applicants for a job with optional filtering (OFO, EmploymentStatus, Province).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetApplicants(int jobId, [FromQuery] string? ofoCode = null, [FromQuery] string? employmentStatus = null, [FromQuery] string? province = null)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Verify employer owns the job
            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null || job.EmployerID != employerId.Value)
                return Forbid();

            // Use filtered applicants if any filter is provided
            var applications = !string.IsNullOrWhiteSpace(ofoCode) || 
                             !string.IsNullOrWhiteSpace(employmentStatus) || 
                             !string.IsNullOrWhiteSpace(province)
                ? await _applicationService.GetFilteredApplicantsForJobAsync(jobId, ofoCode, employmentStatus, province)
                : await _applicationService.GetApplicationsByJobAsync(jobId);
            
            return Ok(applications);
        }

        /// <summary>
        /// Get a specific applicant's application and profile.
        /// </summary>
        [HttpGet("{candidateId:int}")]
        public async Task<IActionResult> GetApplicantProfile(int jobId, int candidateId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null || job.EmployerID != employerId.Value)
                return Forbid();

            var applications = await _applicationService.GetApplicationsByJobAsync(jobId);
            var application = applications.FirstOrDefault(a => a.CandidateID == candidateId);

            if (application == null)
                return NotFound();

            var candidateProfile = await _candidateService.GetCandidateProfileByIdAsync(candidateId);

            return Ok(new
            {
                Application = application,
                Candidate = candidateProfile
            });
        }

        /// <summary>
        /// Update application status.
        /// </summary>
        [HttpPut("{candidateId:int}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(int jobId, int candidateId, [FromBody] UpdateStatusDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null || job.EmployerID != employerId.Value)
                return Forbid();

            var applications = await _applicationService.GetApplicationsByJobAsync(jobId);
            var app = applications.FirstOrDefault(a => a.CandidateID == candidateId);
            if (app == null)
                return NotFound();

            await _applicationService.UpdateApplicationStatusAsync(app.ApplicationID, dto.Status);
            return NoContent();
        }
    }

    public class UpdateStatusDto
    {
        public ApplicationStatus Status { get; set; }
    }
}