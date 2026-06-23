using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;

namespace Obscura.FinanceTracker.WebApi.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        // =============================================================================
        // DATABASE INITIALIZATION
        // =============================================================================
        // Apply pending EF Core migrations and seed initial data at startup.
        //
        // This runs inside a scoped service scope to correctly resolve AppDbContext,
        // which has a Scoped lifetime. Creating a manual scope here is necessary
        // because the application host itself operates at Singleton scope — resolving
        // Scoped services directly from the root provider would cause a runtime error.
        //
        // Behavior by environment:
        //   Development  → Migrations applied + optional database reset + seed data
        //   Production   → Migrations applied only (reset is disabled for safety)
        //
        // The ResetDatabase flag is controlled via appsettings.json:
        //   "SeederOptions": { "ResetDatabase": true }
        // =============================================================================
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            var configuration = services.GetRequiredService<IConfiguration>();

            var resetDatabase = configuration.GetValue<bool>("SeederOptions:ResetDatabase");

            var shouldResetDatabase = resetDatabase && app.Environment.IsDevelopment();

            await DataSeeder.SeedAsync(dbContext, shouldResetDatabase);
        }
    }
}
