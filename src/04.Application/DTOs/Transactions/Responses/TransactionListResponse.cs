using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Responses
{
    public class TransactionListResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType Name { get; set; } 
        public decimal Amount { get; set; }
        public string Type { get; set; } = String.Empty;
        public Guid CategoryId { get; set; }
        public Guid AccountId { get; set; }
    }
}
