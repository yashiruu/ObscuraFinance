namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    /// <summary>
    /// Total expense for a single category, shown on the dashboard.
    /// </summary>
    public class CategoryExpenseResponse
    {
        /// <summary>Name of the category.</summary>
        public string CategoryName { get; set; } = String.Empty;

        /// <summary>Sum of expense transactions in this category.</summary>
        public decimal TotalExpense { get; set; }
    }
}
