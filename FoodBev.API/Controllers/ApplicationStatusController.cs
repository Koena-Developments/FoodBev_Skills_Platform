using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [ApiController]
    [Route("api/v1/applications/{applicationId:int}")]
    [Authorize]
    public class ApplicationStatusController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IJobService _jobService;
        private readonly IEmployerService _employerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApplicationStatusController> _logger;

        private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public ApplicationStatusController(
            IApplicationService applicationService, 
            IJobService jobService,
            IEmployerService employerService,
            IUnitOfWork unitOfWork,
            ILogger<ApplicationStatusController> logger)
        {
            _applicationService = applicationService;
            _jobService = jobService;
            _employerService = employerService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Updates application status (Employer only).
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateStatus(int applicationId, [FromBody] UpdateApplicationStatusDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UpdateStatus called without valid user ID for applicationId: {ApplicationId}", applicationId);
                return Unauthorized(new { message = "User ID is missing." });
            }

            if (dto == null)
            {
                _logger.LogWarning("UpdateStatus called with null DTO for applicationId: {ApplicationId}", applicationId);
                return BadRequest(new { message = "Request body is required", errors = new[] { "DTO cannot be null" } });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();
                
                _logger.LogWarning("ModelState validation failed for applicationId: {ApplicationId}. Errors: {Errors}", 
                    applicationId, string.Join(", ", errors));
                
                return BadRequest(new { message = "Validation failed", errors = errors });
            }

            // Verify employer owns the job associated with this application
            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
            {
                _logger.LogWarning("UpdateStatus called by user {UserId} who is not an employer for applicationId: {ApplicationId}", userId, applicationId);
                return Forbid();
            }

            // Get the application with job details to verify ownership efficiently
            var application = await _unitOfWork.Applications.GetApplicationWithDetailsAsync(applicationId);
            if (application == null)
            {
                _logger.LogWarning("UpdateStatus called for non-existent application {ApplicationId}", applicationId);
                return NotFound(new { message = "Application not found." });
            }

            // Verify the job belongs to this employer
            var job = application.Job ?? await _unitOfWork.JobPostings.GetByIdAsync(application.JobID);
            if (job == null || job.EmployerID != employerId.Value)
            {
                _logger.LogWarning("UpdateStatus called for application {ApplicationId} that doesn't belong to employer {EmployerId}", applicationId, employerId.Value);
                return Forbid();
            }

            _logger.LogInformation("Updating application {ApplicationId} status to {Status} for employer {EmployerId}", applicationId, dto.Status, employerId.Value);

            var success = await _applicationService.UpdateApplicationStatusAsync(applicationId, dto.Status);
            if (!success)
            {
                _logger.LogWarning("Failed to update application {ApplicationId} status - application not found", applicationId);
                return NotFound(new { message = "Application not found." });
            }

            _logger.LogInformation("Successfully updated application {ApplicationId} status to {Status}", applicationId, dto.Status);
            return NoContent();
        }

        /// <summary>
        /// Schedules an interview (Employer only).
        /// </summary>
        [HttpPut("schedule-interview")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ScheduleInterview(int applicationId, [FromBody] ScheduleInterviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _applicationService.ScheduleInterviewAsync(
                applicationId, 
                dto.InterviewDate, 
                dto.InterviewVenue
            );

            if (!success)
            {
                return NotFound("Application not found or unauthorized.");
            }

            return NoContent();
        }

        /// <summary>
        /// Updates interview response (Candidate only).
        /// </summary>
        [HttpPut("interview-response")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UpdateInterviewResponse(int applicationId, [FromBody] UpdateInterviewResponseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _applicationService.UpdateInterviewResponseAsync(applicationId, dto.Response);
            if (!success)
            {
                return NotFound("Application not found or interview not scheduled.");
            }

            return NoContent();
        }
    }

    public class UpdateApplicationStatusDto
    {
        public ApplicationStatus Status { get; set; }
    }

    public class ScheduleInterviewDto
    {
        public DateTime InterviewDate { get; set; }
        public string InterviewVenue { get; set; }
    }

    public class UpdateInterviewResponseDto
    {
        public InterviewResponse Response { get; set; }
    }
}

