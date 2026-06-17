namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    public class DashboardSummaryResponse
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal CurrentBalance { get; set; }
        public int TotalTransaction { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public List<RecentTransactionResponse> RecentTransactions { get; set; } = [];
        public List<CategoryExpenseResponse> CategoryExpenses { get; set; } = [];
        public List<AccountSummaryResponse> AccountSummary { get; set; } = [];
    }
}
