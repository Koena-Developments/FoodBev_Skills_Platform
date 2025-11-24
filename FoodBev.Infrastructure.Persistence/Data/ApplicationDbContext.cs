using FoodBev.Core.Domain.Entities;
// using FoodBev.Core.Application.Interfaces; // New dependency for the interface
using FoodBev.Core.Domain.Common; // New dependency for AuditableEntity
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;
using FoodBev.Core.Domain.Interfaces;

namespace FoodBev.Infrastructure.Persistence.Data // Using the .Data sub-namespace
{
    /// <summary>
    /// The Entity Framework Core DbContext for the FoodBev application.
    /// It implements IApplicationDbContext to expose the persistence layer as an abstraction
    /// to the Application layer, and includes logic for automatic auditing.
    /// </summary>
    public class ApplicationDbContext : DbContext, IApplicationDbContext // Now implementing the interface
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // --- DbSet Definitions: These properties map our C# classes to database tables ---
        public DbSet<User> Users { get; set; }
        public DbSet<CandidateEntity> CandidateDetails { get; set; }
        public DbSet<EmployerEntity> EmployerDetails { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<SkillsProgrammeForm> SkillsProgrammeForms { get; set; }

        public DbSet<OfoCode> OfoCodes { get; set; }
        // public DbSet<SkillsProgrammeForm> SkillsProgrammes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from the current assembly if they were using IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // --- 1. User Entity Configurations ---
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserID);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // --- 2. CandidateDetail Entity Configurations (One-to-One with User) ---
            modelBuilder.Entity<CandidateEntity>(entity =>
            {
                entity.HasKey(c => c.CandidateID); 
                entity.Property(c => c.CandidateID).ValueGeneratedNever(); 
                entity.HasIndex(c => c.IDNumber).IsUnique();
            });
            
            // --- 3. EmployerDetail Entity Configurations (One-to-One with User) ---
            modelBuilder.Entity<EmployerEntity>(entity =>
            {
                entity.HasKey(e => e.EmployerID); 
                entity.Property(e => e.EmployerID).ValueGeneratedNever(); 
                entity.HasIndex(e => e.LevyNumber).IsUnique();
            });

            // --- 4. JobPosting Entity Configurations (One-to-Many with EmployerDetail) ---
            modelBuilder.Entity<JobPosting>(entity =>
            {
                entity.HasKey(j => j.JobID);
                
                entity.HasOne<EmployerEntity>() 
                    .WithMany()
                    .HasForeignKey(j => j.EmployerID)
                    .OnDelete(DeleteBehavior.Restrict); 
            });
            
            // --- 5. Application Entity Configurations (Many-to-Many join table) ---
            modelBuilder.Entity<ApplicationEntity>(entity =>
            {
                entity.HasKey(a => a.ApplicationID);
                
                entity.HasOne(a => a.Job)
                    .WithMany(j => j.Applications)
                    .HasForeignKey(a => a.JobID)
                    .OnDelete(DeleteBehavior.Restrict); 
                
                entity.HasOne(a => a.Candidate)
                    .WithMany(c => c.Applications)
                    .HasForeignKey(a => a.CandidateID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => new { a.JobID, a.CandidateID }).IsUnique();
            });

            // --- 6. SkillsProgrammeForm Entity Configurations (One-to-One with Application) ---
            modelBuilder.Entity<SkillsProgrammeForm>(entity =>
            {
                entity.HasKey(f => f.FormID); 

                entity.HasOne(f => f.Application)
                    .WithOne(a => a.SkillsProgrammeForm)
                    .HasForeignKey<SkillsProgrammeForm>(f => f.ApplicationID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // --- 7. OfoCode Entity Configurations ---
            modelBuilder.Entity<OfoCode>(entity =>
            {
                entity.HasKey(o => o.OfoCodeID);
            });
        }
        
        /// <summary>
        /// Overrides SaveChangesAsync to automatically update AuditableEntity properties (Created/Modified Dates).
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Ensure created date is set on creation
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        // Ensure modified date is set on modification
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        // Mark CreatedDate as not modified to preserve the original creation time
                        entry.Property("CreatedDate").IsModified = false; 
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}