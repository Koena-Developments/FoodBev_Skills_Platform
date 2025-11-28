using FoodBev.Application.Interfaces;
using FoodBev.Infrastructure.Persistence.Services;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence.Data;
using FoodBev.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodBev.Infrastructure.Persistence.Configurations 
{
    /// <summary>
    /// Extension methods for configuring Persistence layer services, specifically the database context and Unit of Work.
    /// This is where the database provider (SQLite) and connection string are registered.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            // 2. Register Repositories and Unit of Work
            // The UnitOfWork allows us to manage transactions across multiple repositories.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 3. Register Security Service for password hashing and JWT token generation
            services.AddScoped<ISecurityService, SecurityService>();

            // 4. Register File Storage Service
            services.AddScoped<IFileStorageService, FileStorageService>();

            // 5. Register specific repositories (e.g., if you had IUserRepository)
            // services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}