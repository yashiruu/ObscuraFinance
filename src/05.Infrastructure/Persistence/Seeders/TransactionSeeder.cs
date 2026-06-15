using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;

public static class TransactionSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (await dbContext.Transactions.AnyAsync())
        {
            return;
        }

        var accounts = await dbContext.Accounts.ToListAsync();
        var categories = await dbContext.Categories.ToListAsync();

        var mandiri = accounts.FirstOrDefault(x => x.Name == "Mandiri")
            ?? throw new InvalidOperationException("Mandiri account not found.");

        var bri = accounts.FirstOrDefault(x => x.Name == "BRI")
            ?? throw new InvalidOperationException("BRI account not found.");

        var ovo = accounts.FirstOrDefault(x => x.Name == "OVO")
            ?? throw new InvalidOperationException("OVO account not found.");

        var salary = categories.FirstOrDefault(x => x.Name == "Salary")
            ?? throw new InvalidOperationException("Salary category not found.");

        var food = categories.FirstOrDefault(x => x.Name == "Food")
            ?? throw new InvalidOperationException("Food category not found.");

        var transportation = categories.FirstOrDefault(x => x.Name == "Transportation")
            ?? throw new InvalidOperationException("Transportation category not found.");

        var grocery = categories.FirstOrDefault(x => x.Name == "Grocery")
            ?? throw new InvalidOperationException("Grocery category not found.");

        var bills = categories.FirstOrDefault(x => x.Name == "Bills")
            ?? throw new InvalidOperationException("Bills category not found.");

        var entertainment = categories.FirstOrDefault(x => x.Name == "Entertainment")
            ?? throw new InvalidOperationException("Entertainment category not found.");

        var savings = categories.FirstOrDefault(x => x.Name == "Savings")
            ?? throw new InvalidOperationException("Savings category not found.");

        var transactions = new List<Transaction>();

        // Generate 6 months of sample data
        for (int month = 0; month < 6; month++)
        {
            var targetMonth = DateTime.Today.AddMonths(-month);

            var baseDate = new DateTime(
                targetMonth.Year,
                targetMonth.Month,
                1);

            // Salary

            transactions.Add(new Transaction
            {
                Date = new DateTime(baseDate.Year, baseDate.Month, 25),
                Name = "Monthly Salary",
                Amount = 6_000_000m,
                Type = TransactionType.Income,
                AccountId = mandiri.Id,
                CategoryId = salary.Id
            });

            // Savings

            transactions.Add(new Transaction
            {
                Date = new DateTime(baseDate.Year, baseDate.Month, 26),
                Name = "Monthly Savings",
                Amount = 1_000_000m,
                Type = TransactionType.Expense,
                AccountId = bri.Id,
                CategoryId = savings.Id
            });

            // Bills

            transactions.Add(new Transaction
            {
                Date = new DateTime(baseDate.Year, baseDate.Month, 1),
                Name = "Boarding House Rent",
                Amount = 1_300_000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = bills.Id
            });

            transactions.Add(new Transaction
            {
                Date = new DateTime(baseDate.Year, baseDate.Month, 5),
                Name = "Internet Package",
                Amount = 200_000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = bills.Id
            });

            // Food

            for (int week = 0; week < 4; week++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(week * 7),
                    Name = "Lunch",
                    Amount = 35_000m,
                    Type = TransactionType.Expense,
                    AccountId = mandiri.Id,
                    CategoryId = food.Id
                });

                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(week * 7 + 1),
                    Name = "Dinner",
                    Amount = 30_000m,
                    Type = TransactionType.Expense,
                    AccountId = mandiri.Id,
                    CategoryId = food.Id
                });

                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(week * 7 + 2),
                    Name = "Coffee",
                    Amount = 25_000m,
                    Type = TransactionType.Expense,
                    AccountId = mandiri.Id,
                    CategoryId = food.Id
                });
            }

            // Transportation

            for (int trip = 0; trip < 8; trip++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(trip * 3),
                    Name = "GoRide",
                    Amount = 20_000m,
                    Type = TransactionType.Expense,
                    AccountId = ovo.Id,
                    CategoryId = transportation.Id
                });
            }

            // Grocery

            for (int groceryTrip = 0; groceryTrip < 2; groceryTrip++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(groceryTrip * 14 + 3),
                    Name = "Mini Market",
                    Amount = 150_000m,
                    Type = TransactionType.Expense,
                    AccountId = mandiri.Id,
                    CategoryId = grocery.Id
                });
            }

            // Entertainment

            transactions.Add(new Transaction
            {
                Date = baseDate.AddDays(10),
                Name = "Movie Ticket",
                Amount = 50_000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = entertainment.Id
            });

            transactions.Add(new Transaction
            {
                Date = baseDate.AddDays(20),
                Name = "Book Purchase",
                Amount = 120_000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = entertainment.Id
            });
        }

        await dbContext.Transactions.AddRangeAsync(transactions);
        await dbContext.SaveChangesAsync();
    }
}