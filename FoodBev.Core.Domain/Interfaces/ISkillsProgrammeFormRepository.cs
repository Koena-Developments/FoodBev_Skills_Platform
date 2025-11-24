using FoodBev.Core.Domain.Entities;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for SkillsProgrammeForm entity, managing form completion/signatures.
    /// </summary>
    public interface ISkillsProgrammeFormRepository : IGenericRepository<SkillsProgrammeForm>
    {
        Task<SkillsProgrammeForm> GetByApplicationIdAsync(int applicationId);
        
        // Methods to check signing status
        Task<bool> HasCandidateSignedAsync(int formId);
    }
}