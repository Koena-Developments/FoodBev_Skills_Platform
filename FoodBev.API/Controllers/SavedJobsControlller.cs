using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/candidate/saved-jobs")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class SavedJobsController : ControllerBase
    {
        private readonly ISavedJobsService _savedJobsService;
        private readonly ICandidateService _candidateService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public SavedJobsController(
            ISavedJobsService savedJobsService,
            ICandidateService candidateService)
        {
            _savedJobsService = savedJobsService;
            _candidateService = candidateService;
        }

        [HttpPost("{jobId:int}")]
        public async Task<IActionResult> SaveJob(int jobId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            await _savedJobsService.SaveJobAsync(candidateId.Value, jobId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetSavedJobs()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            var jobs = await _savedJobsService.GetSavedJobsAsync(candidateId.Value);
            return Ok(jobs);
        }
    }
}