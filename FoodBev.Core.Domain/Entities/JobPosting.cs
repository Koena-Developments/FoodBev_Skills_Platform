using System;
using System.Collections.Generic;

namespace FoodBev.Core.Domain.Entities
{
    /// <summary>
    /// Represents a single job or skills programme vacancy posted by an employer.
    /// </summary>
    public class JobPosting
    {
        public int JobID { get; set; }
        
        // Foreign Key to the posting employer
        public int EmployerID { get; set; } 
        
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string OFO_Code_Required { get; set; } // For matching to candidate OFO codes
        public string? PreferredProvince { get; set; } // Optional: Geographic preference for filtering
        public bool IsBursary { get; set; } // Flag to identify bursary programs
        
        // Dates
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public DateTime ApplicationDeadline { get; set; }
        
        // Navigation Property: Link to all applications for this job
        public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    }
}