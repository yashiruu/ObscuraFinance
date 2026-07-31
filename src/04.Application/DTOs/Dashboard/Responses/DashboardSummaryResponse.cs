namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    /// <summary>
    /// Aggregated financial summary shown on the dashboard.
    /// </summary>
    public class DashboardSummaryResponse
    {
        /// <summary>Sum of all income transactions.</summary>
        public decimal TotalIncome { get; set; }

        /// <summary>Sum of all expense transactions.</summary>
        public decimal TotalExpense { get; set; }

        /// <summary>Sum of the current balance across all active accounts.</summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>Total number of active transactions.</summary>
        public int TotalTransaction { get; set; }

        /// <summary>UTC timestamp at which this summary was generated.</summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>The 10 most recent transactions.</summary>
        public List<RecentTransactionResponse> RecentTransactions { get; set; } = [];

        /// <summary>Total expense grouped by category.</summary>
        public List<CategoryExpenseResponse> CategoryExpenses { get; set; } = [];

        /// <summary>Balance per account.</summary>
        public List<AccountSummaryResponse> AccountSummary { get; set; } = [];
    }
}
