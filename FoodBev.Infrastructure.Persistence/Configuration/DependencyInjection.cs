// using FoodBev.Core.Application.Interfaces;
using FoodBev.Application.Interfaces;
using FoodBev.Infrastructure.Persistence.Services;
using FoodBev.Core.Domain.Interfaces;
using FoodBev.Infrastructure.Persistence;
using FoodBev.Infrastructure.Persistence.Data;
using FoodBev.Infrastructure.Persistence.Repositories;
using FoodBev.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace FoodBev.Infrastructure.Persistence.Configurations // CORRECTED NAMESPACE
{
    /// <summary>
    /// Extension methods for configuring Persistence layer services, specifically the database context and Unit of Work.
    /// This is where the database provider (MySQL) and connection string are registered.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configure the Database Context
            // We'll use MySQL (Pomelo) as the database provider.
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Using MySQL 8.0 server version - adjust if using a different version
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));

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