using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Infrastructure.Services;

namespace Obscura.FinanceTracker.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // =============================================================================
        // APPLICATION SERVICES
        // =============================================================================
        // Register application services with Scoped lifetime.
        // Scoped means one instance is created per HTTP request and shared across
        // all components that resolve it within that same request.
        //
        // Controllers depend on abstractions (interfaces), not concrete implementations.
        // This follows the Dependency Inversion Principle (DIP) from SOLID —
        // high-level modules should not depend on low-level modules; both should
        // depend on abstractions.
        //
        // Example resolution chain:
        //   Controller → ICategoryService → CategoryService → AppDbContext
        // =============================================================================
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
