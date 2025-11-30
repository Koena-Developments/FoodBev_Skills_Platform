using System;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    /// <summary>
    /// Defines the Unit of Work pattern, aggregating all repositories and managing transactions.
    /// Inherits from IDisposable to ensure the DbContext is properly cleaned up.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Expose all concrete repository interfaces
        IUserRepository Users { get; }
        ICandidateRepository Candidates { get; }
        IEmployerRepository Employers { get; }
        IJobPostingRepository JobPostings { get; }
        IApplicationRepository Applications { get; }
        ISkillsProgrammeFormRepository SkillsProgrammeForms { get; }
        ITripartiteAgreementRepository TripartiteAgreements { get; }
         IOfoCodeRepository OfoCodes { get; }
         ISkillsProgrammeRepository SkillsProgrammes { get; }
        // IApplicationRepository Applications { get; }
        /// <summary>
        /// Saves all pending changes in the unit of work (across all repositories) to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> CompleteAsync();
    }
}