using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    public class RecentTransactionResponse
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = String.Empty;
        public TransactionType Type { get; set; }
        public string Category { get; set; } = String.Empty;
        public string Account { get; set; } = String.Empty;
        public decimal Amount { get; set; }
    }
}
