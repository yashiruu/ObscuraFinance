using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Seeders
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (await dbContext.Categories.AnyAsync())
            {
                return;
            }

            await dbContext.Categories.AddRangeAsync(GetCategories());

            await dbContext.SaveChangesAsync();
        }

        public static List<Category> GetCategories()
        {
            return
            [
                // Income

                new()
            {
                Name = "Salary",
                Description = "Monthly salary",
                Type = TransactionType.Income
            },

            new()
            {
                Name = "Bonus",
                Description = "Additional income",
                Type = TransactionType.Income
            },

            new()
            {
                Name = "Freelance",
                Description = "Freelance projects",
                Type = TransactionType.Income
            },

            // Expense

            new()
            {
                Name = "Food",
                Description = "Meals and dining",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Transportation",
                Description = "Transport expenses",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Bills",
                Description = "Monthly bills",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Grocery",
                Description = "Daily necessities",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Entertainment",
                Description = "Movies, games, hobbies",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Personal Care",
                Description = "Health and hygiene",
                Type = TransactionType.Expense
            },

            new()
            {
                Name = "Savings",
                Description = "Savings allocation",
                Type = TransactionType.Expense
            }
            ];
        }
    }
}