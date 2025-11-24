using FoodBev.Application.Interfaces;
using FoodBev.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBev.Application.Services
{
    /// <summary>
    /// Implements lookup services for static/reference data like OFO codes and provinces.
    /// </summary>
    public class LookupService : ILookupService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<object>> GetOfoCodesAsync(string query = null)
        {
            var codes = await _unitOfWork.OfoCodes.GetAllAsync();
            var codesList = codes.ToList();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var queryLower = query.ToLower();
                codesList = codesList.Where(c => 
                    c.Code.ToLower().Contains(queryLower) || 
                    c.Description.ToLower().Contains(queryLower) ||
                    (!string.IsNullOrEmpty(c.Sector) && c.Sector.ToLower().Contains(queryLower))).ToList();
            }

            return codesList.Select(c => new
            {
                c.OfoCodeID,
                c.Code,
                c.Description,
                c.Sector
            });
        }

        public Task<IEnumerable<string>> GetProvincesAsync()
        {
            // South African provinces
            var provinces = new List<string>
            {
                "Eastern Cape",
                "Free State",
                "Gauteng",
                "KwaZulu-Natal",
                "Limpopo",
                "Mpumalanga",
                "Northern Cape",
                "North West",
                "Western Cape"
            };

            return Task.FromResult<IEnumerable<string>>(provinces);
        }
    }
}

