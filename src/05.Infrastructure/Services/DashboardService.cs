using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(AppDbContext context, ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving dashboard summary");

            // Query for Summary Card
            var totalIncome = await _context.Transactions
                .Where(t => t.Type == TransactionType.Income && !t.IsDeleted)
                .SumAsync(t => t.Amount);

            var totalExpense = await _context.Transactions
                .Where(t => t.Type == TransactionType.Expense && !t.IsDeleted)
                .SumAsync(t => t.Amount);

            var currentBalance = await _context.Accounts
                .Where(a => !a.IsDeleted)
                .SumAsync(a => a.CurrentBalance);

            var totalTransaction = await _context.Transactions
                .CountAsync(t => !t.IsDeleted);

            // Query for Recent Transaction
            var recentTransaction = await _context.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.Date)
                .Take(10)
                .Select(t => new RecentTransactionResponse  // Projection
                {
                    Id = t.Id,
                    TransactionDate = t.Date,
                    Description = t.Name,
                    Type = t.Type,
                    Category = t.Category!.Name,
                    Account = t.Account!.Name,
                    Amount = t.Amount
                }).ToListAsync();

            // Query for Expense by Category
            var categoryExpenses = await _context.Transactions
                .Include(t => t.Category)
                .Where(t =>
                    !t.IsDeleted &&
                    t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category!.Name)
                .Select(t => new CategoryExpenseResponse    // Projection
                {
                    CategoryName = t.Key,
                    TotalExpense = t.Sum(t => t.Amount)
                })
                .OrderByDescending(t => t.TotalExpense)
                .ToListAsync();

            // Query for Account Summary
            var accountSummary = await _context.Accounts
                .Where(a => !a.IsDeleted && a.CurrentBalance != 0)
                .OrderByDescending(a => a.CurrentBalance)
                .Select(a => new AccountSummaryResponse     // Projection
                {
                    Id = a.Id,
                    AccountName = a.Name,
                    Balance = a.CurrentBalance
                })
                .ToListAsync();

            _logger.LogInformation(
                "Dashboard summary retrieved successfully. Income: {TotalIncome}, Expense: {TotalExpense}, Transactions: {TotalTransaction}",
                totalIncome,
                totalExpense,
                totalTransaction);

            return new DashboardSummaryResponse
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                CurrentBalance = currentBalance,
                TotalTransaction = totalTransaction,
                LastUpdated = DateTime.UtcNow,

                RecentTransactions = recentTransaction,
                CategoryExpenses = categoryExpenses,
                AccountSummary = accountSummary
            };
        }
    }
}
