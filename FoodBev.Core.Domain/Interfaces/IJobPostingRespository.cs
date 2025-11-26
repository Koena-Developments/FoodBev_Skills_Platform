using FoodBev.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for JobPosting entity.
    /// </summary>
    public interface IJobPostingRepository : IGenericRepository<JobPosting>
    {
        Task<IEnumerable<JobPosting>> GetActiveJobsAsync();
        
        // Custom query to find jobs matching a candidate's profile (e.g., OFO code, province, bursary status)
        Task<IEnumerable<JobPosting>> GetMatchingJobsAsync(string candidateOfoCode, string candidateProvince, bool isBursarySeeker);
        
        Task<IEnumerable<JobPosting>> GetJobsByEmployerAsync(int employerId);
    }
}