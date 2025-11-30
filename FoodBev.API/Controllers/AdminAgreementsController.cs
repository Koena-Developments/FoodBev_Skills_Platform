using FoodBev.Application.DTOs.TripartiteAgreement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/admin/agreements")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAgreementsController : ControllerBase
    {
        private readonly ITripartiteAgreementService _agreementService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public AdminAgreementsController(ITripartiteAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        /// <summary>
        /// Gets all agreements pending admin review.
        /// </summary>
        [HttpGet("pending-review")]
        public async Task<IActionResult> GetPendingReview()
        {
            var agreements = await _agreementService.GetPendingAdminReviewAsync();
            return Ok(agreements);
        }

        /// <summary>
        /// Gets a specific agreement by ID.
        /// </summary>
        [HttpGet("{agreementId:int}")]
        public async Task<IActionResult> GetAgreement(int agreementId)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(agreementId);
            if (agreement == null)
                return NotFound();

            return Ok(agreement);
        }

        /// <summary>
        /// Admin reviews and approves/rejects an agreement.
        /// </summary>
        [HttpPost("{agreementId:int}/review")]
        public async Task<IActionResult> ReviewAgreement(int agreementId, [FromBody] AdminReviewAgreementDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return BadRequest(new { errors });
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _agreementService.ReviewAgreementAsync(agreementId, userId, dto.Approved, dto.Notes);
            if (!success)
                return BadRequest("Unable to review agreement. Agreement may not exist or is not in the correct status.");

            return Ok(new { message = dto.Approved ? "Agreement approved successfully." : "Agreement rejected." });
        }
    }
}

