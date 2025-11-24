using FoodBev.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    public interface IOfoCodeRepository : IGenericRepository<OfoCode>
    {
        // Add custom methods if needed, e.g.:
        // Task<OfoCode> GetByCodeAsync(string code);
    }
}