// FoodBev.Core.Application/Configurations/DependencyInjection.cs
using Microsoft.Extensions.DependencyInjection;
using FoodBev.Application.Interfaces;
using FoodBev.Application.Services;

namespace FoodBev.Core.Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreApplicationServices(this IServiceCollection services)
        {
            // Register all your application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ISetaAdminService, SetaAdminService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISavedJobsService, SavedJobsService>();
            services.AddScoped<ITripartiteAgreementService, TripartiteAgreementService>();

            return services;
        }
    }
}