using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/employer/{employerId:int}/public-profile")]
    [ApiController]
    public class PublicEmployerProfileController : ControllerBase
    {
        private readonly IEmployerService _employerService;
        private readonly IJobService _jobService;

        public PublicEmployerProfileController(
            IEmployerService employerService,
            IJobService jobService)
        {
            _employerService = employerService;
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublicProfile(int employerId)
        {
            var employer = await _employerService.GetEmployerProfileByIdAsync(employerId);
            if (employer == null)
                return NotFound();

            var activeJobs = await _jobService.GetJobsByEmployerAsync(employerId);

            return Ok(new
            {
                employer.CompanyName,
                employer.LevyNumber,
                employer.SDFName,
                LogoUrl = "TODO", // Add logo field to DTO
                Jobs = activeJobs
            });
        }
    }
}