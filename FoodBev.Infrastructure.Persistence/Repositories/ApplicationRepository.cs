using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Enums;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of IApplicationRepository.
    /// </summary>
    public class ApplicationRepository : GenericRepository<ApplicationEntity>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Retrieves a single application, eagerly loading related Job and Candidate details.
        /// </summary>
        public async Task<ApplicationEntity> GetApplicationWithDetailsAsync(int applicationId)
        {
            return await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Include(a => a.SkillsProgrammeForm) // Also include the form status
                .FirstOrDefaultAsync(a => a.ApplicationID == applicationId);
        }

        /// <summary>
        /// Retrieves all applications made by a specific candidate.
        /// </summary>
        public async Task<IEnumerable<ApplicationEntity>> GetApplicationsByCandidateAsync(int candidateId)
        {
            return await _context.Applications
                .Where(a => a.CandidateID == candidateId)
                .Include(a => a.Job) // Include job title for context
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all applications for a specific job posting.
        /// </summary>
        public async Task<IEnumerable<ApplicationEntity>> GetApplicationsByJobAsync(int jobId)
        {
            return await _context.Applications
                .Where(a => a.JobID == jobId)
                .Include(a => a.Candidate) // Include candidate details for review
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all applications matching a specific status (e.g., Shortlisted).
        /// </summary>
        public async Task<IEnumerable<ApplicationEntity>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            return await _context.Applications
                .Where(a => a.Status == status)
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Include(a => a.SkillsProgrammeForm)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a candidate has already applied to a specific job.
        /// </summary>
        public async Task<ApplicationEntity> GetApplicationByJobAndCandidateAsync(int jobId, int candidateId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.JobID == jobId && a.CandidateID == candidateId);
        }
    }
}