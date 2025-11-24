using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of IJobPostingRepository.
    /// </summary>
    public class JobPostingRepository : GenericRepository<JobPosting>, IJobPostingRepository
    {
        public JobPostingRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves all job postings that are still active (application deadline is in the future).
        /// </summary>
        public async Task<IEnumerable<JobPosting>> GetActiveJobsAsync()
        {
            return await _context.JobPostings
                .Where(j => j.ApplicationDeadline > DateTime.UtcNow)
                .OrderByDescending(j => j.DatePosted)
                .ToListAsync();
        }
        
        /// <summary>
        /// Finds jobs posted by a specific employer.
        /// </summary>
        public async Task<IEnumerable<JobPosting>> GetJobsByEmployerAsync(int employerId)
        {
            return await _context.JobPostings
                .Where(j => j.EmployerID == employerId)
                .OrderByDescending(j => j.DatePosted)
                .ToListAsync();
        }

        /// <summary>
        /// Finds jobs that match a candidate's profile based on OFO code and bursary status.
        /// </summary>
        public async Task<IEnumerable<JobPosting>> GetMatchingJobsAsync(string candidateOfoCode, bool isBursarySeeker)
        {
            // Logic to match jobs:
            // 1. Job's required OFO code matches the candidate's OFO code OR
            // 2. The job is a bursary (IsBursary = true), which may bypass the OFO match
            return await _context.JobPostings
                .Where(j => j.ApplicationDeadline > DateTime.UtcNow) // Must be active
                .Where(j => j.OFO_Code_Required == candidateOfoCode || j.IsBursary == isBursarySeeker)
                .OrderByDescending(j => j.DatePosted)
                .ToListAsync();
        }
    }
}