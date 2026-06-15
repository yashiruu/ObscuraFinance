using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Seeders
{
    public  class AccountSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (await dbContext.Accounts.AnyAsync())
            {
                return;
            }

            await dbContext.Accounts.AddRangeAsync(GetAccounts());

            await dbContext.SaveChangesAsync();
        }

        public static List<Account> GetAccounts()
        {
            return [
                new Account()
                {
                    Name = "Cash",
                    Description = "Physical cash",
                    InitialBalance = 500_000m,
                    CurrentBalance = 250_000m,
                    Currency = "IDR",
                    Type = AccountType.Cash,
                    IsActive = true
                },

                new Account()
                {
                    Name = "Mandiri",
                    Description = "Daily spending account",
                    InitialBalance = 5_000_000m,
                    CurrentBalance = 2_500_000m,
                    Currency = "IDR",
                    Type = AccountType.Bank,
                    IsActive = true
                },

                new Account()
                {
                    Name = "BRI",
                    Description = "Savings account",
                    InitialBalance = 10_000_000m,
                    CurrentBalance = 8_000_000m,
                    Currency = "IDR",
                    Type = AccountType.Bank,
                    IsActive = true
                },

                new Account()
                {
                    Name = "OVO",
                    Description = "Transportation wallet",
                    InitialBalance = 300_000m,
                    CurrentBalance = 150_000m,
                    Currency = "IDR",
                    Type = AccountType.EWallet,
                    IsActive = true
                },

                new Account()
                {
                    Name = "Gopay",
                    Description = "Transportation wallet",
                    InitialBalance = 300_000m,
                    CurrentBalance = 150_000m,
                    Currency = "IDR",
                    Type = AccountType.EWallet,
                    IsActive = true
                }
            ];
        }
    }
}
