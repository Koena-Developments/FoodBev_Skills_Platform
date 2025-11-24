using FoodBev.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodBev.API.Controllers
{
    [ApiController]
    [Route("api/v1/lookups")]
    [AllowAnonymous] // All lookups are generally public data
    public class LookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        /// <summary>
        /// Retrieves the list of OFO (Organising Framework for Occupations) codes.
        /// </summary>
        [HttpGet("ofo-codes")]
        public async Task<IActionResult> GetOfoCodes([FromQuery] string query)
        {
            var codes = await _lookupService.GetOfoCodesAsync(query);
            return Ok(codes);
        }

        /// <summary>
        /// Retrieves a list of available Provinces.
        /// </summary>
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _lookupService.GetProvincesAsync();
            return Ok(provinces);
        }
    }
}