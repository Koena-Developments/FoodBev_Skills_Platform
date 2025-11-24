using FoodBev.Application.DTOs.ApplicationManagement;
using FoodBev.Core.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for managing job applications and the associated Skills Programme Forms.
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// Creates a new job application for a candidate, including the associated Skills Form.
        /// </summary>
        Task<ApplicationSummaryDto> ApplyToJobAsync(CreateApplicationDto dto);

        /// <summary>
        /// Retrieves all applications submitted by a candidate.
        /// </summary>
        Task<IEnumerable<ApplicationSummaryDto>> GetApplicationsByCandidateAsync(int candidateId);

        /// <summary>
        /// Retrieves all applications for a specific job (used by the employer).
        /// </summary>
        Task<IEnumerable<ApplicationSummaryDto>> GetApplicationsByJobAsync(int jobId);

        /// <summary>
        /// Updates the status of an application (e.g., Shortlisted, Hired).
        /// </summary>
        Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus);
        
        // --- Skills Programme Form Management ---

        /// <summary>
        /// Retrieves the Skills Programme Form associated with a specific application.
        /// </summary>
        Task<SkillsFormDto> GetSkillsProgrammeFormAsync(int applicationId);

        /// <summary>
        /// Candidate submits their digital signature to the form.
        /// </summary>
        Task<bool> SubmitCandidateSignatureAsync(SignatureDto signatureDto);

        /// <summary>
        /// Employer submits their digital signature to the form.
        /// </summary>
        Task<bool> SubmitEmployerSignatureAsync(SignatureDto signatureDto);
    }
}