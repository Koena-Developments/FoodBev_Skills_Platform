using FoodBev.Application.DTOs.TripartiteAgreement;
using FoodBev.Application.Interfaces;
using FoodBev.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
        private readonly PdfGenerationService _pdfService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public AdminAgreementsController(
            ITripartiteAgreementService agreementService,
            PdfGenerationService pdfService)
        {
            _agreementService = agreementService;
            _pdfService = pdfService;
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

        /// <summary>
        /// Gets complete form details including all candidate, employer, and agreement information.
        /// </summary>
        [HttpGet("{agreementId:int}/complete-details")]
        public async Task<IActionResult> GetCompleteFormDetails(int agreementId)
        {
            var formDetails = await _agreementService.GetCompleteFormDetailsAsync(agreementId);
            if (formDetails == null)
                return NotFound(new { error = "Agreement not found" });

            return Ok(formDetails);
        }

        /// <summary>
        /// Downloads the form as PDF with complete details and signatures.
        /// PDFs are saved in date-organized folders (format: yyyy-MM-dd).
        /// </summary>
        [HttpGet("{agreementId:int}/download-pdf")]
        public async Task<IActionResult> DownloadFormPdf(int agreementId)
        {
            var formDetails = await _agreementService.GetCompleteFormDetailsAsync(agreementId);
            if (formDetails == null)
                return NotFound();

            try
            {
                // Generate PDF
                var pdfBytes = _pdfService.GenerateFormPdf(formDetails);

                // Save PDF to date-organized folder
                var dateFolder = DateTime.UtcNow.ToString("yyyy-MM-dd");
                var fileName = $"Agreement_{formDetails.AgreementID}_{formDetails.CandidateFirstName}_{formDetails.CandidateLastName}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
                var folderPath = Path.Combine("form-pdfs", dateFolder);
                
                // Ensure directory exists
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
                Directory.CreateDirectory(fullPath);

                var filePath = Path.Combine(fullPath, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

                // Return PDF as download
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to generate PDF", details = ex.Message });
            }
        }
    }
}

