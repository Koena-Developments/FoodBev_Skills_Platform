using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public ApplicationStatusController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Updates application status (Employer only).
        /// </summary>
        [HttpPut("status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateStatus(int applicationId, [FromBody] UpdateApplicationStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _applicationService.UpdateApplicationStatusAsync(applicationId, dto.Status);
            if (!success)
            {
                return NotFound("Application not found or unauthorized.");
            }

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

