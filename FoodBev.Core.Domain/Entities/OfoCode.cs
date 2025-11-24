using System.ComponentModel.DataAnnotations;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Data Transfer Object for transferring OFO Code information, typically for bulk uploads or listings.
    /// </summary>
    public class OfoCode
    {
        /// <summary>
        /// The unique OFO Code.
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        public int OfoCodeID { get; set; }

        /// <summary>
        /// The description of the occupation.
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public string Sector { get; set; } = string.Empty;
    }
}