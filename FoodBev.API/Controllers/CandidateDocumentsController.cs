using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/candidate/documents")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidateDocumentsController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ICandidateService _candidateService;

        public CandidateDocumentsController(
            IFileStorageService fileStorageService,
            ICandidateService candidateService)
        {
            _fileStorageService = fileStorageService;
            _candidateService = candidateService;
        }

        /// <summary>
        /// Uploads an ID document for the candidate.
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file uploaded or file is empty." });
            }

            // Validate file type (only allow PDF, images)
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { error = "Invalid file type. Only PDF, JPG, JPEG, and PNG files are allowed." });
            }

            // Validate file size (max 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxFileSize)
            {
                return BadRequest(new { error = "File size exceeds the maximum limit of 5MB." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                // Get candidate ID
                var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
                if (!candidateId.HasValue)
                {
                    return NotFound(new { error = "Candidate profile not found." });
                }

                // Read file bytes
                using var memoryStream = new System.IO.MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                // Upload file
                var documentUrl = await _fileStorageService.UploadFileAsync("candidate-documents", fileBytes, file.FileName);

                // Update candidate profile with document reference
                var profile = await _candidateService.GetCandidateProfileByUserIdAsync(userId);
                if (profile != null)
                {
                    var updateDto = new FoodBev.Application.DTOs.ProfileManagement.UpdateCandidateProfileDto
                    {
                        ID_Document_Ref = documentUrl
                    };
                    await _candidateService.UpdateCandidateProfileAsync(candidateId.Value, updateDto);
                }

                return Ok(new
                {
                    message = "Document uploaded successfully.",
                    documentUrl = documentUrl,
                    fileName = file.FileName
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while uploading the document.", details = ex.Message });
            }
        }

        /// <summary>
        /// Gets the candidate's uploaded documents.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var profile = await _candidateService.GetCandidateProfileByUserIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { error = "Candidate profile not found." });
            }

            var documents = new List<object>();

            if (!string.IsNullOrWhiteSpace(profile.ID_Document_Ref))
            {
                documents.Add(new
                {
                    type = "ID_Document",
                    url = profile.ID_Document_Ref,
                    uploaded = true
                });
            }

            return Ok(new { documents });
        }

        /// <summary>
        /// Deletes a document.
        /// </summary>
        [HttpDelete("{docType}")]
        public async Task<IActionResult> DeleteDocument(string docType)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var candidateId = await _candidateService.GetCandidateIdByUserIdAsync(userId);
            if (!candidateId.HasValue)
            {
                return NotFound(new { error = "Candidate profile not found." });
            }

            var profile = await _candidateService.GetCandidateProfileByUserIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { error = "Candidate profile not found." });
            }

            try
            {
                string documentUrl = null;
                if (docType.ToLowerInvariant() == "id_document" || docType.ToLowerInvariant() == "id")
                {
                    documentUrl = profile.ID_Document_Ref;
                }

                if (string.IsNullOrWhiteSpace(documentUrl))
                {
                    return NotFound(new { error = "Document not found." });
                }

                // Delete file from storage
                await _fileStorageService.DeleteFileAsync(documentUrl);

                // Update profile to remove document reference
                var updateDto = new FoodBev.Application.DTOs.ProfileManagement.UpdateCandidateProfileDto();
                if (docType.ToLowerInvariant() == "id_document" || docType.ToLowerInvariant() == "id")
                {
                    updateDto.ID_Document_Ref = null;
                }

                await _candidateService.UpdateCandidateProfileAsync(candidateId.Value, updateDto);

                return Ok(new { message = "Document deleted successfully." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the document.", details = ex.Message });
            }
        }
    }
}