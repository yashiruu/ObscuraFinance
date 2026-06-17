namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    public class AccountSummaryResponse
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; } = String.Empty;
        public decimal Balance { get; set; }
    }
}
