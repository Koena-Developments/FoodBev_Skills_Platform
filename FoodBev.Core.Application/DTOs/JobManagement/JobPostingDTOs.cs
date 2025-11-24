using System;
using System.ComponentModel.DataAnnotations;

namespace FoodBev.Application.DTOs.JobManagement
{
    /// <summary>
    /// DTO used for creating a new job posting.
    /// </summary>
    public class CreateJobPostingDto
    {
        [Required]
        public int EmployerID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string JobTitle { get; set; }
        
        [Required]
        public string JobDescription { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string OFO_Code_Required { get; set; }
        
        public bool IsBursary { get; set; } = false;
        
        [Required]
        public DateTime ApplicationDeadline { get; set; }
    }

    /// <summary>
    /// DTO for reading and displaying job posting details.
    /// </summary>
    public class JobPostingDto
    {
        public int JobID { get; set; }
        public int EmployerID { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string OFO_Code_Required { get; set; }
        public bool IsBursary { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string CompanyName { get; set; } // To be populated from EmployerDetail
    }

    /// <summary>
    /// DTO for searching jobs with various criteria.
    /// </summary>
    public class JobSearchDto
    {
        public string? Query { get; set; }
        public string? OFO_Code { get; set; }
        public bool? IsBursary { get; set; }
        public int? EmployerID { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing job posting.
    /// </summary>
    public class UpdateJobPostingDto
    {
        [MaxLength(100)]
        public string? JobTitle { get; set; }
        
        public string? JobDescription { get; set; }
        
        [MaxLength(10)]
        public string? OFO_Code_Required { get; set; }
        
        public bool? IsBursary { get; set; }
        
        public DateTime? ApplicationDeadline { get; set; }
    }
}