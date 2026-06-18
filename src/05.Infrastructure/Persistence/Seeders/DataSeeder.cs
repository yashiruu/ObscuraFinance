namespace Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, bool resetDatabase = false)
    {
        if (resetDatabase)
        {
            await ResetDataAsync(dbContext);
        }

        await CategorySeeder.SeedAsync(dbContext);

        await AccountSeeder.SeedAsync(dbContext);

        await TransactionSeeder.SeedAsync(dbContext);
    }

    private static async Task ResetDataAsync(AppDbContext dbContext)
    {
        dbContext.Transactions.RemoveRange(dbContext.Transactions);

        dbContext.Accounts.RemoveRange(dbContext.Accounts);

        dbContext.Categories.RemoveRange(dbContext.Categories);

        await dbContext.SaveChangesAsync();
    }
}