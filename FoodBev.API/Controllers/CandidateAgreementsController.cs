using FoodBev.Application.DTOs.TripartiteAgreement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/candidate/agreements")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidateAgreementsController : ControllerBase
    {
        private readonly ITripartiteAgreementService _agreementService;
        private readonly ICandidateService _candidateService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public CandidateAgreementsController(
            ITripartiteAgreementService agreementService,
            ICandidateService candidateService)
        {
            _agreementService = agreementService;
            _candidateService = candidateService;
        }

        /// <summary>
        /// Gets all agreements for the current candidate.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyAgreements()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
                return Forbid();

            var agreements = await _agreementService.GetAgreementsByCandidateIdAsync(candidateId.Value);
            return Ok(agreements);
        }

        /// <summary>
        /// Gets a specific agreement by ID.
        /// </summary>
        [HttpGet("{agreementId:int}")]
        public async Task<IActionResult> GetAgreement(int agreementId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var agreement = await _agreementService.GetAgreementByIdAsync(agreementId);
            if (agreement == null)
                return NotFound();

            // Verify the agreement belongs to this candidate
            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue || agreement.CandidateID != candidateId.Value)
                return Forbid();

            return Ok(agreement);
        }

        /// <summary>
        /// Candidate signs the agreement with a digital signature.
        /// </summary>
        [HttpPost("{agreementId:int}/sign")]
        public async Task<IActionResult> SignAgreement(int agreementId, [FromBody] SignAgreementDto dto)
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

            var success = await _agreementService.SignAsCandidateAsync(agreementId, userId, dto.SignatureBase64);
            if (!success)
                return BadRequest("Unable to sign agreement. Agreement may not exist or is not in the correct status.");

            return Ok(new { message = "Agreement signed successfully." });
        }
    }
}

