using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        await CategorySeeder.SeedAsync(dbContext);

        await AccountSeeder.SeedAsync(dbContext);

        await TransactionSeeder.SeedAsync(dbContext);
    }
}