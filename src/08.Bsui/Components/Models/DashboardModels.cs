namespace Obscura.FinanceTracker.Bsui.Components.Models;

public class DashboardSummaryResponse
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal CurrentBalance { get; set; }
    public int TotalTransactions { get; set; }
    public List<RecentTransactionDto> RecentTransactions { get; set; } = [];
    public List<CategoryExpenseDto> CategoryExpenses { get; set; } = [];
    public List<AccountSummaryDto> Accounts { get; set; } = [];
}

public class RecentTransactionDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty; // "Income" | "Expense"
}

public class CategoryExpenseDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalExpense { get; set; }
}

public class AccountSummaryDto
{
    public string AccountName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}