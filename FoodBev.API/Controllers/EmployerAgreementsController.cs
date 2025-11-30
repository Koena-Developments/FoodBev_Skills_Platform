using FoodBev.Application.DTOs.TripartiteAgreement;
using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/employer/agreements")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerAgreementsController : ControllerBase
    {
        private readonly ITripartiteAgreementService _agreementService;
        private readonly IEmployerService _employerService;
        private readonly IFileStorageService _fileStorageService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public EmployerAgreementsController(
            ITripartiteAgreementService agreementService,
            IEmployerService employerService,
            IFileStorageService fileStorageService)
        {
            _agreementService = agreementService;
            _employerService = employerService;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Gets all agreements for the current employer.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyAgreements()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            var agreements = await _agreementService.GetAgreementsByEmployerIdAsync(employerId.Value);
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

            // Verify the agreement belongs to this employer
            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue || agreement.EmployerID != employerId.Value)
                return Forbid();

            return Ok(agreement);
        }

        /// <summary>
        /// Employer signs the agreement and uploads Training Provider signature file.
        /// </summary>
        [HttpPost("{agreementId:int}/sign")]
        public async Task<IActionResult> SignAgreement(int agreementId, [FromForm] string employerSignatureBase64, [FromForm] IFormFile? trainingProviderSignatureFile)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(employerSignatureBase64))
            {
                return BadRequest(new { message = "Employer signature is required." });
            }

            // Upload Training Provider signature file if provided
            string? tpSignatureFileRef = null;
            if (trainingProviderSignatureFile != null)
            {
                // Validate file type and size
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var fileExtension = System.IO.Path.GetExtension(trainingProviderSignatureFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type. Only PDF, JPG, JPEG, and PNG files are allowed." });
                }

                if (trainingProviderSignatureFile.Length > 10 * 1024 * 1024) // 10MB
                {
                    return BadRequest(new { message = "File size exceeds 10MB limit." });
                }

                // Convert IFormFile to byte array
                using var memoryStream = new System.IO.MemoryStream();
                await trainingProviderSignatureFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                // Save the file
                var fileName = $"tp-signature-{agreementId}{fileExtension}";
                tpSignatureFileRef = await _fileStorageService.UploadFileAsync(
                    "tripartite-agreements",
                    fileBytes,
                    fileName
                );
            }

            var success = await _agreementService.SignAsEmployerAsync(
                agreementId, 
                userId, 
                employerSignatureBase64, 
                tpSignatureFileRef
            );

            if (!success)
                return BadRequest("Unable to sign agreement. Agreement may not exist or is not in the correct status.");

            return Ok(new { message = "Agreement signed and submitted successfully." });
        }
    }
}

