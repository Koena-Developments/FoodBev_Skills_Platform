using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using System;
using System.Threading.Tasks;

namespace FoodBev.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Concrete implementation of the Unit of Work pattern.
    /// It encapsulates the application's DbContext and provides access to all repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        // Private backing fields for repository instances
        private IUserRepository _userRepository;
        private ICandidateRepository _candidateRepository;
        private IEmployerRepository _employerRepository;
        private IJobPostingRepository _jobPostingRepository;
        private IApplicationRepository _applicationRepository;
        private ISkillsProgrammeFormRepository _skillsProgrammeFormRepository;
        private ISkillsProgrammeRepository _skillsProgrammeRepository;
        private IOfoCodeRepository _ofoCodeRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Public properties to access the repositories. 
        // We use lazy initialization (??=) to ensure a repository is only created when first accessed.
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICandidateRepository Candidates => _candidateRepository ??= new CandidateRepository(_context);
        public IEmployerRepository Employers => _employerRepository ??= new EmployerRepository(_context);
        public IJobPostingRepository JobPostings => _jobPostingRepository ??= new JobPostingRepository(_context);
        public IApplicationRepository Applications => _applicationRepository ??= new ApplicationRepository(_context);
        public ISkillsProgrammeFormRepository SkillsProgrammeForms => _skillsProgrammeFormRepository ??= new SkillsProgrammeFormRepository(_context);
        public ISkillsProgrammeRepository SkillsProgrammes => _skillsProgrammeRepository ??= new SkillsProgrammeFormRepository(_context);
        public IOfoCodeRepository OfoCodes => _ofoCodeRepository ??= new OfoCodeRepository(_context);

        /// <summary>
        /// Commits all changes tracked by the DbContext to the database.
        /// </summary>
        public Task<int> CompleteAsync()
        {
            // The repositories call Add, Update, Delete methods, but only the UoW calls SaveChangesAsync.
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the DbContext instance to free up resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}