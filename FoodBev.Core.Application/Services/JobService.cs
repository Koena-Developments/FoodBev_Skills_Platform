using FoodBev.Application.DTOs.JobManagement;
using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements the core business logic for managing job postings and matching.
    /// </summary>
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Helper method to convert Entity to DTO and fetch Company Name
        private async Task<JobPostingDto> MapToDto(JobPosting job)
        {
            if (job == null) return null;

            // Fetch Employer Details to get the Company Name
            var employer = await _unitOfWork.Employers.GetByIdAsync(job.EmployerID);
            string companyName = employer?.CompanyName ?? "Unknown Company";

            return new JobPostingDto
            {
                JobID = job.JobID,
                EmployerID = job.EmployerID,
                JobTitle = job.JobTitle,
                JobDescription = job.JobDescription,
                OFO_Code_Required = job.OFO_Code_Required,
                IsBursary = job.IsBursary,
                DatePosted = job.DatePosted,
                ApplicationDeadline = job.ApplicationDeadline,
                CompanyName = companyName
            };
        }

        public async Task<JobPostingDto> CreateJobPostingAsync(CreateJobPostingDto dto)
        {
            // 1. Basic Validation: Ensure the EmployerID exists
            var employer = await _unitOfWork.Employers.GetByIdAsync(dto.EmployerID);
            if (employer == null)
            {
                // Could return a DTO response with an error message instead of null for better error handling
                return null; 
            }

            // 2. Map DTO to Entity
            var jobPosting = new JobPosting
            {
                EmployerID = dto.EmployerID,
                JobTitle = dto.JobTitle,
                JobDescription = dto.JobDescription,
                OFO_Code_Required = dto.OFO_Code_Required,
                IsBursary = dto.IsBursary,
                ApplicationDeadline = dto.ApplicationDeadline,
                // DatePosted is set by the entity's default value (DateTime.UtcNow)
            };

            // 3. Add and Save
            await _unitOfWork.JobPostings.AddAsync(jobPosting);
            await _unitOfWork.CompleteAsync();

            // 4. Return DTO of the newly created job, including the company name
            return await MapToDto(jobPosting);
        }

        public async Task<JobPostingDto> GetJobByIdAsync(int jobId)
        {
            var job = await _unitOfWork.JobPostings.GetByIdAsync(jobId);
            return await MapToDto(job);
        }

        public async Task<IEnumerable<JobPostingDto>> GetActiveJobPostingsAsync()
        {
            var jobs = await _unitOfWork.JobPostings.GetActiveJobsAsync();
            var jobDtos = new List<JobPostingDto>();

            // Convert and enrich each entity with the company name
            foreach (var job in jobs)
            {
                jobDtos.Add(await MapToDto(job));
            }

            return jobDtos;
        }

        public async Task<IEnumerable<JobPostingDto>> GetJobsByEmployerAsync(int employerId)
        {
            var jobs = await _unitOfWork.JobPostings.GetJobsByEmployerAsync(employerId);
            var jobDtos = new List<JobPostingDto>();

            // Since all these jobs are for the same employer, fetch the name once for efficiency.
            var employer = await _unitOfWork.Employers.GetByIdAsync(employerId);
            string companyName = employer?.CompanyName ?? "Unknown Company";

            foreach (var job in jobs)
            {
                jobDtos.Add(new JobPostingDto
                {
                    JobID = job.JobID,
                    EmployerID = job.EmployerID,
                    JobTitle = job.JobTitle,
                    JobDescription = job.JobDescription,
                    OFO_Code_Required = job.OFO_Code_Required,
                    IsBursary = job.IsBursary,
                    DatePosted = job.DatePosted,
                    ApplicationDeadline = job.ApplicationDeadline,
                    CompanyName = companyName
                });
            }

            return jobDtos;
        }

        public async Task<IEnumerable<JobPostingDto>> GetMatchingJobsForCandidateAsync(int candidateId)
        {
            // 1. Get Candidate's profile for matching criteria
            var candidate = await _unitOfWork.Candidates.GetByIdAsync(candidateId);

            if (candidate == null || string.IsNullOrWhiteSpace(candidate.OFO_Code))
            {
                // Cannot match without an OFO code, return empty set.
                return new List<JobPostingDto>(); 
            }

            // 2. Fetch matching jobs from the repository
            // Assuming IsBursarySeeker is true if the candidate is unemployed, 
            // making them eligible for certain bursary programmes.
            var jobs = await _unitOfWork.JobPostings.GetMatchingJobsAsync(candidate.OFO_Code, candidate.EmploymentStatus == "Unemployed"); 

            var jobDtos = new List<JobPostingDto>();
            
            // 3. Convert and enrich with Company Name
            foreach (var job in jobs)
            {
                jobDtos.Add(await MapToDto(job));
            }

            return jobDtos;
        }

        public async Task<bool> DeleteJobPostingAsync(int jobId)
        {
            var job = await _unitOfWork.JobPostings.GetByIdAsync(jobId);

            if (job == null)
            {
                return false;
            }

            // Implementation note: The DbContext is configured to RESTRICT deletion if applications exist.
            // We rely on the database to enforce this constraint for now.
            try
            {
                _unitOfWork.JobPostings.Delete(job);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch
            {
                // Catch any database constraint violation (e.g., existing applications)
                return false; 
            }
        }

        public async Task<IEnumerable<JobPostingDto>> SearchJobsAsync(JobSearchDto search)
        {
            var allJobs = await _unitOfWork.JobPostings.GetActiveJobsAsync();
            var filteredJobs = allJobs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.Query))
            {
                var query = search.Query.ToLower();
                filteredJobs = filteredJobs.Where(j => 
                    j.JobTitle.ToLower().Contains(query) || 
                    j.JobDescription.ToLower().Contains(query));
            }

            if (!string.IsNullOrWhiteSpace(search.OFO_Code))
            {
                filteredJobs = filteredJobs.Where(j => j.OFO_Code_Required == search.OFO_Code);
            }

            if (search.IsBursary.HasValue)
            {
                filteredJobs = filteredJobs.Where(j => j.IsBursary == search.IsBursary.Value);
            }

            if (search.EmployerID.HasValue)
            {
                filteredJobs = filteredJobs.Where(j => j.EmployerID == search.EmployerID.Value);
            }

            var jobList = filteredJobs.ToList();
            var jobDtos = new List<JobPostingDto>();

            foreach (var job in jobList)
            {
                jobDtos.Add(await MapToDto(job));
            }

            return jobDtos;
        }

        public async Task<int> CreateJobAsync(string userId, CreateJobPostingDto dto)
        {
            // Get employer ID from user ID
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new InvalidOperationException("Employer profile not found for the authenticated user.");
            }

            // Set the employer ID from the authenticated user
            dto.EmployerID = employer.EmployerID;

            var result = await CreateJobPostingAsync(dto);
            return result?.JobID ?? 0;
        }

        public async Task<bool> UpdateJobAsync(string userId, int jobId, UpdateJobPostingDto dto)
        {
            // Get employer ID from user ID
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return false;
            }

            // Verify the job belongs to this employer
            var job = await _unitOfWork.JobPostings.GetByIdAsync(jobId);
            if (job == null || job.EmployerID != employer.EmployerID)
            {
                return false;
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(dto.JobTitle))
                job.JobTitle = dto.JobTitle;

            if (!string.IsNullOrWhiteSpace(dto.JobDescription))
                job.JobDescription = dto.JobDescription;

            if (!string.IsNullOrWhiteSpace(dto.OFO_Code_Required))
                job.OFO_Code_Required = dto.OFO_Code_Required;

            if (dto.IsBursary.HasValue)
                job.IsBursary = dto.IsBursary.Value;

            if (dto.ApplicationDeadline.HasValue)
                job.ApplicationDeadline = dto.ApplicationDeadline.Value;

            _unitOfWork.JobPostings.Update(job);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteJobAsync(string userId, int jobId)
        {
            // Get employer ID from user ID
            var employer = await _unitOfWork.Employers.GetByUserIdAsync(userId);
            if (employer == null)
            {
                return false;
            }

            // Verify the job belongs to this employer
            var job = await _unitOfWork.JobPostings.GetByIdAsync(jobId);
            if (job == null || job.EmployerID != employer.EmployerID)
            {
                return false;
            }

            return await DeleteJobPostingAsync(jobId);
        }

        public async Task<IEnumerable<JobPostingDto>> GetMatchingJobsForCandidateByUserIdAsync(string userId)
        {
            // Get candidate by user ID
            var candidate = await _unitOfWork.Candidates.GetByUserIdAsync(userId);
            if (candidate == null)
            {
                return new List<JobPostingDto>();
            }

            return await GetMatchingJobsForCandidateAsync(candidate.CandidateID);
        }
    }
}