using FoodBev.Application.DTOs.JobManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing saved jobs for candidates.
    /// </summary>
    public interface ISavedJobsService
    {
        /// <summary>
        /// Saves a job for a candidate.
        /// </summary>
        Task SaveJobAsync(int candidateId, int jobId);

        /// <summary>
        /// Removes a saved job for a candidate.
        /// </summary>
        Task RemoveSavedJobAsync(int candidateId, int jobId);

        /// <summary>
        /// Retrieves all saved jobs for a candidate.
        /// </summary>
        Task<IEnumerable<JobPostingDto>> GetSavedJobsAsync(int candidateId);

        /// <summary>
        /// Checks if a job is saved by a candidate.
        /// </summary>
        Task<bool> IsJobSavedAsync(int candidateId, int jobId);
    }
}

