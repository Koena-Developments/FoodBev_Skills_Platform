using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for lookup services (OFO codes, provinces, etc.).
    /// </summary>
    public interface ILookupService
    {
        /// <summary>
        /// Retrieves OFO codes, optionally filtered by query string.
        /// </summary>
        Task<IEnumerable<object>> GetOfoCodesAsync(string query = null);

        /// <summary>
        /// Retrieves a list of available provinces.
        /// </summary>
        Task<IEnumerable<string>> GetProvincesAsync();
    }
}

