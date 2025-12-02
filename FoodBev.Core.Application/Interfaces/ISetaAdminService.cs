using FoodBev.Application.DTOs.AdminManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for SETA administrative functions, including OFO management and application vetting.
    /// </summary>
    public interface ISetaAdminService
    {
        // --- OFO Code Management ---

        /// <summary>
        /// Creates a new OFO code entry in the database.
        /// </summary>
        Task<OfoCodeDto> CreateOfoCodeAsync(CreateOfoCodeDto dto);

        /// <summary>
        /// Retrieves a list of all OFO codes.
        /// </summary>
        Task<IEnumerable<OfoCodeDto>> GetAllOfoCodesAsync();

        /// <summary>
        /// Retrieves an OFO code by its unique ID.
        /// </summary>
        Task<OfoCodeDto> GetOfoCodeByIdAsync(int ofoCodeId);

        /// <summary>
        /// Updates an existing OFO code entry.
        /// </summary>
        Task<OfoCodeDto> UpdateOfoCodeAsync(int ofoCodeId, CreateOfoCodeDto dto);

        /// <summary>
        /// Deletes an OFO code entry.
        /// </summary>
        Task<bool> DeleteOfoCodeAsync(int ofoCodeId);

        // --- Application Vetting ---

        /// <summary>
        /// Retrieves all applications that are awaiting SETA review (e.g., status is 'Submitted').
        /// </summary>
        Task<IEnumerable<ApplicationReviewDto>> GetPendingApplicationsAsync();

        /// <summary>
        /// Retrieves a specific application for detailed review.
        /// </summary>
        Task<ApplicationReviewDto> GetApplicationForReviewAsync(int applicationId);

        /// <summary>
        /// Updates the status of an application (e.g., Approved, Rejected, Registered).
        /// </summary>
        Task<ApplicationReviewDto> UpdateApplicationStatusAsync(int applicationId, UpdateApplicationStatusDto dto);

        // --- Dashboard Statistics ---

        /// <summary>
        /// Gets dashboard statistics (Total Students, Applications, Active Students, Funded Companies).
        /// </summary>
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        /// <summary>
        /// Gets student demographics grouped by province.
        /// </summary>
        Task<IEnumerable<DemographicsDto>> GetDemographicsByProvinceAsync();

        /// <summary>
        /// Gets recent activity feed.
        /// </summary>
        Task<IEnumerable<ActivityDto>> GetRecentActivityAsync(int limit = 50);

        /// <summary>
        /// Gets application trends grouped by date (last 30 days by default).
        /// </summary>
        Task<IEnumerable<ApplicationTrendDto>> GetApplicationTrendsAsync(int days = 30);
    }
}