using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Responses
{
    public class TransactionDetailResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = String.Empty;
        public Guid AccountId { get; set; }
        public string AccountName { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
