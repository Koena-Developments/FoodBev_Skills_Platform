using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/candidate/applications")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidateApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ICandidateService _candidateService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public CandidateApplicationsController(
            IApplicationService applicationService,
            ICandidateService candidateService)
        {
            _applicationService = applicationService;
            _candidateService = candidateService;
        }

        /// <summary>
        /// Submits an application for a specific job ID.
        /// </summary>
        [HttpPost("{jobId:int}")]
        public async Task<IActionResult> ApplyToJob(int jobId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            var dto = new CreateApplicationDto
            {
                JobID = jobId,
                CandidateID = candidateId.Value,
                CV_File_Ref = "", // Or fetch from user profile
                CandidateAvailability = "Immediate"
            };

            try
            {
                var result = await _applicationService.ApplyToJobAsync(dto);
                if (result == null)
                    return BadRequest("Job or candidate not found.");

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Handle duplicate application or other business logic errors
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all applications for the current candidate.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            var applications = await _applicationService.GetApplicationsByCandidateAsync(candidateId.Value);
            return Ok(applications);
        }

        /// <summary>
        /// Withdraws an application.
        /// </summary>
        [HttpDelete("{jobId:int}")]
        public async Task<IActionResult> WithdrawApplication(int jobId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            var applications = await _applicationService.GetApplicationsByCandidateAsync(candidateId.Value);
            var app = applications.FirstOrDefault(a => a.JobID == jobId);
            if (app == null)
                return NotFound();

            await _applicationService.UpdateApplicationStatusAsync(app.ApplicationID, ApplicationStatus.Rejected);
            return NoContent();
        }
    }
}