using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/employer/logo")]
    [ApiController]
    [Authorize(Roles = "Employer")]
    public class EmployerLogoController : ControllerBase
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IEmployerService _employerService;

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public EmployerLogoController(
            IFileStorageService fileStorage,
            IEmployerService employerService)
        {
            _fileStorage = fileStorage;
            _employerService = employerService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadLogo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var employerId = await _employerService.GetEmployerIdByUserIdAsync(userId);
            if (!employerId.HasValue)
                return Forbid();

            // Read file bytes
            using var memoryStream = new System.IO.MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var logoUrl = await _fileStorage.UploadFileAsync("logos", fileBytes, file.FileName);
            
            // Update employer profile with logo URL
            var employer = await _employerService.GetEmployerProfileByIdAsync(employerId.Value);
            // TODO: Add LogoUrl property to EmployerProfileDto and UpdateEmployerProfileDto
            
            return Ok(new { LogoUrl = logoUrl });
        }
    }
}