using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for Application entity, managing the job application process.
    /// </summary>
    public interface IApplicationRepository : IGenericRepository<ApplicationEntity>
    {
        Task<IEnumerable<ApplicationEntity>> GetApplicationsByCandidateAsync(int candidateId);
        Task<IEnumerable<ApplicationEntity>> GetApplicationsByJobAsync(int jobId);
        Task<IEnumerable<ApplicationEntity>> GetApplicationsByStatusAsync(ApplicationStatus status);
        
        Task<ApplicationEntity> GetApplicationWithDetailsAsync(int applicationId);
        
        /// <summary>
        /// Checks if a candidate has already applied to a specific job.
        /// </summary>
        Task<ApplicationEntity> GetApplicationByJobAndCandidateAsync(int jobId, int candidateId);
    }
}