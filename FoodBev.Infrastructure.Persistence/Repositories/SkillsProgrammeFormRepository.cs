using FoodBev.Core.Domain.Entities;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of ISkillsProgrammeFormRepository, focusing on form-specific checks.
    /// Also implements ISkillsProgrammeRepository since both work with SkillsProgrammeForm.
    /// </summary>
    public class SkillsProgrammeFormRepository : GenericRepository<SkillsProgrammeForm>, ISkillsProgrammeFormRepository, ISkillsProgrammeRepository
    {
        public SkillsProgrammeFormRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a form based on the associated Application ID.
        /// </summary>
        public async Task<SkillsProgrammeForm> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.SkillsProgrammeForms
                .FirstOrDefaultAsync(f => f.ApplicationID == applicationId);
        }

        /// <summary>
        /// Checks if the candidate's signature field is populated for a given form.
        /// </summary>
        public async Task<bool> HasCandidateSignedAsync(int formId)
        {
            // Checks if the signature string is not null or empty
            return await _context.SkillsProgrammeForms
                .Where(f => f.FormID == formId)
                .AnyAsync(f => !string.IsNullOrEmpty(f.CandidateSignature));
        }
    }
}