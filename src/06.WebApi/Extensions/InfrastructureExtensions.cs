using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.WebApi.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            // =============================================================================
            // DATABASE CONFIGURATION
            // =============================================================================
            // Register AppDbContext with the dependency injection container.
            // EF Core manages the DbContext lifetime — one instance is created per HTTP
            // request (Scoped lifetime) and disposed when the request ends.
            //
            // The connection string is read from appsettings.json under:
            //   "ConnectionStrings": { "DefaultConnection": "..." }
            // =============================================================================
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            // Unit of Work — coordinates all repositories under one DbContext
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
