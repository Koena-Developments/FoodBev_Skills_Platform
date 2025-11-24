using FoodBev.Application.DTOs.JobManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing job postings, including creation, retrieval, and matching.
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Creates a new job posting based on the provided DTO.
        /// </summary>
        Task<JobPostingDto> CreateJobPostingAsync(CreateJobPostingDto dto);

        /// <summary>
        /// Retrieves a specific job posting by ID.
        /// </summary>
        Task<JobPostingDto> GetJobByIdAsync(int jobId);

        /// <summary>
        /// Retrieves all currently active job postings.
        /// </summary>
        Task<IEnumerable<JobPostingDto>> GetActiveJobPostingsAsync();

        /// <summary>
        /// Retrieves jobs posted by a specific employer.
        /// </summary>
        Task<IEnumerable<JobPostingDto>> GetJobsByEmployerAsync(int employerId);

        /// <summary>
        /// Finds and retrieves jobs that match a candidate's profile (OFO code, bursary status).
        /// </summary>
        Task<IEnumerable<JobPostingDto>> GetMatchingJobsForCandidateAsync(int candidateId);
        
        /// <summary>
        /// Deletes a job posting.
        /// </summary>
        Task<bool> DeleteJobPostingAsync(int jobId);

        /// <summary>
        /// Searches for jobs based on various criteria.
        /// </summary>
        Task<IEnumerable<JobPostingDto>> SearchJobsAsync(JobSearchDto search);

        /// <summary>
        /// Creates a new job posting for a specific user (employer).
        /// </summary>
        Task<int> CreateJobAsync(string userId, CreateJobPostingDto dto);

        /// <summary>
        /// Updates an existing job posting for a specific user (employer).
        /// </summary>
        Task<bool> UpdateJobAsync(string userId, int jobId, UpdateJobPostingDto dto);

        /// <summary>
        /// Deletes a job posting for a specific user (employer).
        /// </summary>
        Task<bool> DeleteJobAsync(string userId, int jobId);
    }
}