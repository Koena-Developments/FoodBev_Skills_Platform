// File: FoodBev.Core.Domain/Interfaces/IApplicationDbContext.cs
using FoodBev.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodBev.Core.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
       DbSet<User> Users { get; set; } 
        DbSet<CandidateEntity> CandidateDetails { get; }
        DbSet<EmployerEntity> EmployerDetails { get; }
        DbSet<JobPosting> JobPostings { get; }
        DbSet<ApplicationEntity> Applications { get; }
        DbSet<SkillsProgrammeForm> SkillsProgrammeForms { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}