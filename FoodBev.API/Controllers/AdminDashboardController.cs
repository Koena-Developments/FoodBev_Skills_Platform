using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly ISetaAdminService _adminService;
        private readonly ICandidateService _candidateService;
        private readonly IApplicationService _applicationService;
        private readonly IEmployerService _employerService;

        public AdminDashboardController(
            ISetaAdminService adminService,
            ICandidateService candidateService,
            IApplicationService applicationService,
            IEmployerService employerService)
        {
            _adminService = adminService;
            _candidateService = candidateService;
            _applicationService = applicationService;
            _employerService = employerService;
        }

        /// <summary>
        /// Gets dashboard statistics (Total Students, Applications, Active Students, Funded Companies).
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Gets student demographics grouped by province.
        /// </summary>
        [HttpGet("demographics")]
        public async Task<IActionResult> GetDemographics()
        {
            var demographics = await _adminService.GetDemographicsByProvinceAsync();
            return Ok(demographics);
        }

        /// <summary>
        /// Gets recent activity feed (applications, interviews, hires).
        /// </summary>
        [HttpGet("recent-activity")]
        public async Task<IActionResult> GetRecentActivity([FromQuery] int limit = 50)
        {
            var activities = await _adminService.GetRecentActivityAsync(limit);
            return Ok(activities);
        }

        /// <summary>
        /// Gets application trends grouped by date (last 30 days by default).
        /// </summary>
        [HttpGet("application-trends")]
        public async Task<IActionResult> GetApplicationTrends([FromQuery] int days = 30)
        {
            var trends = await _adminService.GetApplicationTrendsAsync(days);
            return Ok(trends);
        }
    }
}

