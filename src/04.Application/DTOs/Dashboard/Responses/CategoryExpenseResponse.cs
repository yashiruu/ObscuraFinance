namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    public class CategoryExpenseResponse
    {
        public string CategoryName { get; set; } = String.Empty;
        public decimal TotalExpense { get; set; }
    }
}
