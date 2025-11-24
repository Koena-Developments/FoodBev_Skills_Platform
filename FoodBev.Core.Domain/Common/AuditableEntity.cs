using System;

namespace FoodBev.Core.Domain.Common
{
    /// <summary>
    /// Base class for all domain entities that require tracking of creation and modification.
    /// All entities should inherit from this class to ensure consistency in auditing fields.
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Primary key for the entity.
        /// </summary>
        public virtual int Id { get; set; }

        // --- Auditing properties ---
        
        /// <summary>
        /// Identifier of the user who created the entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Date and time the entity was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Identifier of the user who last modified the entity.
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Date and time the entity was last modified. Nullable if never modified.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}