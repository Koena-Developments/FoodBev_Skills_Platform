using FoodBev.API.Configurations;
using Microsoft.EntityFrameworkCore;
using FoodBev.Infrastructure.Persistence.Data;
using Microsoft.Extensions.Logging;
using MySqlConnector;

// --- 1. Builder Initialization ---
var builder = WebApplication.CreateBuilder(args);

// --- 2. Service Registration ---
// This calls the extension method defined in DependencyInjection.cs 
// and registers all services from Infrastructure, Application, and API layers.
builder.Services.AddAPIServices(builder.Configuration);

var app = builder.Build();

// --- 3. Middleware Pipeline Configuration ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger documentation and Swagger UI in development
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Optional: Automatically run database migrations on startup in development
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            // Get the required services
            var serviceProvider = scope.ServiceProvider;
            var dataContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Check if database can be connected
            if (dataContext.Database.CanConnect())
            {
                // Apply any pending migrations.
                logger.LogInformation("Applying database migrations...");
                dataContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            else
            {
                logger.LogWarning("Cannot connect to database. Please ensure MySQL is running and the connection string is correct.");
            }
            
            // Optional: Seed the database with initial data here if needed
            // await FoodBevDbContextSeed.SeedAsync(dataContext);
        }
    }
    catch (MySqlConnector.MySqlException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
    {
        // Log migration error with helpful message
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, 
            "Failed to connect to MySQL database. " +
            "Please ensure MySQL is running on localhost:3306. " +
            "You can start MySQL using: 'net start MySQL' or through the Windows service manager. " +
            "The application will continue to run, but database operations will fail until MySQL is available.");
    }
    catch (Exception ex)
    {
        // Log other migration errors
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseHttpsRedirection();

// Enable CORS policy (Assuming the policy is named "AllowSpecificOrigin" as per your dependency setup)
app.UseCors("AllowSpecificOrigin");

// Use built-in Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to endpoints
app.MapControllers();

app.Run();