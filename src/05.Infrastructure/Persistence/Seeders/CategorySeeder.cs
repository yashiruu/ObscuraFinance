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
                // ===== Income =====
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

                new()
                {
                    Name = "Interest",
                    Description = "Bank interest income",
                    Type = TransactionType.Income
                },

                // ===== Expense =====
                // Daily Living
                new()
                {
                    Name = "Food",
                    Description = "Meals and dining",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Coffee",
                    Description = "Coffee and beverages",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Grocery",
                    Description = "Daily necessities",
                    Type = TransactionType.Expense
                },

                // Transportation
                new()
                {
                    Name = "Transportation",
                    Description = "Transport expenses",
                    Type = TransactionType.Expense
                },

                // Housing & Utilitites
                new()
                {
                    Name = "Rent",
                    Description = "Monthly rent",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Internet",
                    Description = "Internet and mobile data",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Bills",
                    Description = "Utility bills",
                    Type = TransactionType.Expense
                },

                // Lifestyle
                new()
                {
                    Name = "Shopping",
                    Description = "Personal shopping",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Entertainment",
                    Description = "Movies, games and hobbies",
                    Type = TransactionType.Expense
                },

                // Health
                new()
                {
                    Name = "Healthcare",
                    Description = "Medical and healthcare expenses",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Personal Care",
                    Description = "Health and hygiene",
                    Type = TransactionType.Expense
                },

                // Development
                new()
                {
                    Name = "Education",
                    Description = "Courses, books and learning",
                    Type = TransactionType.Expense
                },

                // Finance
                new()
                {
                    Name = "Savings",
                    Description = "Savings allocation",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Investment",
                    Description = "Investment allocation",
                    Type = TransactionType.Expense
                },

                // Social
                new()
                {
                    Name = "Charity",
                    Description = "Donations and charity",
                    Type = TransactionType.Expense
                },

                new()
                {
                    Name = "Gift",
                    Description = "Gifts and celebrations",
                    Type = TransactionType.Expense
                },
            ];
        }
    }
}