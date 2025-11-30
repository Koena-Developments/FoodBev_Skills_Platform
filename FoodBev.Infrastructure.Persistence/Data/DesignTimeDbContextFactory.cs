using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FoodBev.Infrastructure.Persistence.Data
{
    /// <summary>
    /// Factory for creating ApplicationDbContext at design time (for EF Core migrations).
    /// This is required when running migrations from a different project than the startup project.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Try to find appsettings.json in the API project
            // When running from Infrastructure.Persistence, we need to go up two levels
            var currentDir = Directory.GetCurrentDirectory();
            var solutionDir = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
            var apiProjectPath = Path.Combine(solutionDir, "FoodBev.API");
            
            // If that doesn't exist, try alternative paths
            if (!Directory.Exists(apiProjectPath))
            {
                // Try going up from current directory
                var altPath = Path.GetFullPath(Path.Combine(currentDir, "..", "FoodBev.API"));
                if (Directory.Exists(altPath))
                {
                    apiProjectPath = altPath;
                }
                else
                {
                    // Fallback: use current directory and look for appsettings.json
                    apiProjectPath = currentDir;
                }
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Fallback connection string if not found in config
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Data Source=FoodBevDb.db";
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

