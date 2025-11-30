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
    /// Concrete implementation of ITripartiteAgreementRepository.
    /// </summary>
    public class TripartiteAgreementRepository : GenericRepository<TripartiteAgreement>, ITripartiteAgreementRepository
    {
        public TripartiteAgreementRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<TripartiteAgreement?> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.TripartiteAgreements
                .Include(a => a.Application)
                    .ThenInclude(app => app.Job)
                .Include(a => a.Application)
                    .ThenInclude(app => app.Candidate)
                .FirstOrDefaultAsync(a => a.ApplicationID == applicationId);
        }

        public async Task<IEnumerable<TripartiteAgreement>> GetByCandidateIdAsync(int candidateId)
        {
            return await _context.TripartiteAgreements
                .Include(a => a.Application)
                    .ThenInclude(app => app.Job)
                .Where(a => a.Application.CandidateID == candidateId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TripartiteAgreement>> GetByEmployerIdAsync(int employerId)
        {
            return await _context.TripartiteAgreements
                .Include(a => a.Application)
                    .ThenInclude(app => app.Job)
                .Include(a => a.Application)
                    .ThenInclude(app => app.Candidate)
                .Where(a => a.Application.Job.EmployerID == employerId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TripartiteAgreement>> GetPendingAdminReviewAsync()
        {
            return await _context.TripartiteAgreements
                .Include(a => a.Application)
                    .ThenInclude(app => app.Job)
                .Include(a => a.Application)
                    .ThenInclude(app => app.Candidate)
                .Where(a => a.Status == TripartiteAgreementStatus.SubmittedToAdmin)
                .OrderByDescending(a => a.SubmittedToAdminDate)
                .ToListAsync();
        }
    }
}

