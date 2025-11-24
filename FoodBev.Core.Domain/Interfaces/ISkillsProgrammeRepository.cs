using FoodBev.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    public interface ISkillsProgrammeRepository : IGenericRepository<SkillsProgrammeForm>
    {
        // Add custom methods if needed later, e.g.:
        // Task<IEnumerable<SkillsProgramme>> GetActiveProgrammesAsync();
    }
}