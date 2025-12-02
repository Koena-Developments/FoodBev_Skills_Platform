using FoodBev.API.Converters;
using FoodBev.Core.Application.Configurations;
using FoodBev.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace FoodBev.API.Configurations
{
    /// <summary>
    /// Extension methods for configuring application services and dependencies in the API layer.
    /// This centralizes the registration of services from all layers.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Add Infrastructure Services (DB Context, Repositories, UnitOfWork, Auth)
            // The connection string is read from appsettings.json via the API project's IConfiguration.
            services.AddInfrastructureServices(configuration);

            // 2. Add Core Application Services (Business Logic Services, Validators, Mappers)
            services.AddCoreApplicationServices();

            // 2.5. Add API-specific services
            services.AddScoped<FoodBev.API.Services.PdfGenerationService>();

            // 3. Configure JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForHS256";
            var issuer = jwtSettings["Issuer"] ?? "FoodBevAPI";
            var audience = jwtSettings["Audience"] ?? "FoodBevClient";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero, // Remove delay of token when expire
                    // Set RoleClaimType to "role" so ASP.NET Core knows where to find roles
                    RoleClaimType = "role",
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };
                
                // Ensure role claims are properly mapped from JWT to User claims
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var identity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                        if (identity != null)
                        {
                            // Find the role claim - check both "role" and full URI formats
                            var roleClaim = identity.FindFirst("role") 
                                ?? identity.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                                ?? identity.FindFirst(System.Security.Claims.ClaimTypes.Role);
                            
                            if (roleClaim != null)
                            {
                                // Remove ALL existing role claims (both types) to avoid duplicates
                                var allRoleClaims = identity.Claims
                                    .Where(c => c.Type == "role" || 
                                               c.Type == System.Security.Claims.ClaimTypes.Role ||
                                               c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                                    .ToList();
                                
                                foreach (var existing in allRoleClaims)
                                {
                                    identity.RemoveClaim(existing);
                                }
                                
                                // CRITICAL: Add the role claim with BOTH claim types to ensure it works
                                // Add with ClaimTypes.Role (what [Authorize(Roles = "...")] checks)
                                identity.AddClaim(new System.Security.Claims.Claim(
                                    System.Security.Claims.ClaimTypes.Role, 
                                    roleClaim.Value));
                                
                                // Also keep the "role" claim for RoleClaimType compatibility
                                if (roleClaim.Type != "role")
                                {
                                    identity.AddClaim(new System.Security.Claims.Claim("role", roleClaim.Value));
                                }
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Configure Authorization to use roles from JWT claims
            services.AddAuthorization(options =>
            {
                // This ensures role-based authorization works with JWT tokens
                options.FallbackPolicy = null; // No default policy - explicit authorization required
                
                // Ensure role-based policies work correctly
                options.AddPolicy("Candidate", policy => policy.RequireRole("Candidate"));
                options.AddPolicy("Employer", policy => policy.RequireRole("Employer"));
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            // 4. Configure API documentation (Swagger/OpenAPI)
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodBev API", Version = "v1" });
                
                // Add JWT Bearer token support in Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Configure file upload support for Swagger
                // Map IFormFile to binary format for Swagger UI
                c.MapType<IFormFile>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });
                c.MapType<IFormFileCollection>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });
                // Operation filter handles file uploads in RequestBody
                c.OperationFilter<FileUploadOperationFilter>();
            });

            // 5. Add API Controllers with JSON options
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configure JSON serialization to handle camelCase from frontend
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    // Configure enum parsing to be case-insensitive - allows "Shortlisted", "shortlisted", "SHORTLISTED", etc.
                    options.JsonSerializerOptions.Converters.Add(new CaseInsensitiveEnumConverterFactory());
                });
            
            // 6. Configure CORS (Cross-Origin Resource Sharing)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        // WARNING: For production, replace "*" with your specific frontend domain.
                        .AllowAnyOrigin() 
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            return services;
        }
    }
}