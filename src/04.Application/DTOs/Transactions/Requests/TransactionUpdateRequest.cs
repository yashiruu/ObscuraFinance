using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Requests
{
    public class TransactionUpdateRequest
    {
        public DateTime Date { get; set; }
        public string Name { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AccountId { get; set; }

        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
    }
}
