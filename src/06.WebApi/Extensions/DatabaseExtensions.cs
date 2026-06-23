using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.WebApi.Extensions
{
    public static class DatabaseExtensions
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
        public static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
