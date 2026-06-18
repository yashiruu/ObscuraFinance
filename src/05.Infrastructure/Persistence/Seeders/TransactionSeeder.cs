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

            // Food (16 transactions/month)

            var foodTransactions = new[]
            {
                ("Lunch", 35000m),
                ("Dinner", 30000m),
                ("Coffee", 25000m),
                ("Fried Rice", 28000m),
                ("Chicken Rice", 32000m),
                ("Iced Coffee", 22000m),
                ("Bakso", 25000m),
                ("Noodles", 18000m),
                ("Cafe Visit", 45000m),
                ("Weekend Brunch", 55000m),
                ("Tea", 12000m),
                ("Snack", 15000m),
                ("Burger", 40000m),
                ("Pizza Slice", 35000m),
                ("Milk Tea", 28000m),
                ("Late Dinner", 38000m)
            };

            for (int i = 0; i < foodTransactions.Length; i++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays(i + 1),
                    Name = foodTransactions[i].Item1,
                    Amount = foodTransactions[i].Item2,
                    Type = TransactionType.Expense,
                    AccountId = mandiri.Id,
                    CategoryId = food.Id
                });
            }

            // Transportation (12 transactions/month)

            for (int trip = 0; trip < 12; trip++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays((trip * 2) + 1),
                    Name = trip % 2 == 0
                        ? "GoRide"
                        : "GoCar",
                    Amount = trip % 2 == 0
                        ? 18000m
                        : 25000m,
                    Type = TransactionType.Expense,
                    AccountId = ovo.Id,
                    CategoryId = transportation.Id
                });
            }

            // Grocery (4 transactions/month)

            var groceryAmounts = new[]
            {
                120000m,
                185000m,
                145000m,
                210000m
            };

            for (int i = 0; i < groceryAmounts.Length; i++)
            {
                transactions.Add(new Transaction
                {
                    Date = baseDate.AddDays((i * 7) + 3),
                    Name = "Mini Market",
                    Amount = groceryAmounts[i],
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
                Amount = 50000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = entertainment.Id
            });

            transactions.Add(new Transaction
            {
                Date = baseDate.AddDays(18),
                Name = "Book Purchase",
                Amount = 120000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = entertainment.Id
            });

            transactions.Add(new Transaction
            {
                Date = baseDate.AddDays(24),
                Name = "Streaming Subscription",
                Amount = 60000m,
                Type = TransactionType.Expense,
                AccountId = mandiri.Id,
                CategoryId = entertainment.Id
            });
        }

        await dbContext.Transactions.AddRangeAsync(transactions);
        await dbContext.SaveChangesAsync();
    }
}