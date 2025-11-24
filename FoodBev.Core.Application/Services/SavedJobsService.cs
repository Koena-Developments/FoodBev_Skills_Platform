using FoodBev.Application.DTOs.JobManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements saved jobs service using in-memory storage.
    /// In production, create a SavedJob entity and repository.
    /// </summary>
    public class SavedJobsService : ISavedJobsService
    {
        private readonly IUnitOfWork _unitOfWork;
        // In-memory storage (replace with database in production)
        private static readonly Dictionary<int, HashSet<int>> _savedJobs = new();

        public SavedJobsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task SaveJobAsync(int candidateId, int jobId)
        {
            if (!_savedJobs.ContainsKey(candidateId))
                _savedJobs[candidateId] = new HashSet<int>();

            _savedJobs[candidateId].Add(jobId);
            return Task.CompletedTask;
        }

        public Task RemoveSavedJobAsync(int candidateId, int jobId)
        {
            if (_savedJobs.ContainsKey(candidateId))
                _savedJobs[candidateId].Remove(jobId);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<JobPostingDto>> GetSavedJobsAsync(int candidateId)
        {
            if (!_savedJobs.ContainsKey(candidateId) || _savedJobs[candidateId].Count == 0)
                return Enumerable.Empty<JobPostingDto>();

            var jobIds = _savedJobs[candidateId].ToList();
            var jobs = new List<JobPostingDto>();

            foreach (var jobId in jobIds)
            {
                var job = await _unitOfWork.JobPostings.GetByIdAsync(jobId);
                if (job != null)
                {
                    // Get employer for company name
                    var employer = await _unitOfWork.Employers.GetByIdAsync(job.EmployerID);
                    jobs.Add(new JobPostingDto
                    {
                        JobID = job.JobID,
                        EmployerID = job.EmployerID,
                        JobTitle = job.JobTitle,
                        JobDescription = job.JobDescription,
                        OFO_Code_Required = job.OFO_Code_Required,
                        IsBursary = job.IsBursary,
                        DatePosted = job.DatePosted,
                        ApplicationDeadline = job.ApplicationDeadline,
                        CompanyName = employer?.CompanyName ?? "Unknown Company"
                    });
                }
            }

            return jobs;
        }

        public Task<bool> IsJobSavedAsync(int candidateId, int jobId)
        {
            if (!_savedJobs.ContainsKey(candidateId))
                return Task.FromResult(false);

            return Task.FromResult(_savedJobs[candidateId].Contains(jobId));
        }
    }
}

