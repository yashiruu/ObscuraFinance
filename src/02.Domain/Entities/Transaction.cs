using Obscura.FinanceTracker.Base.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }

        // Navigation Property
        public Account? Account { get; set; }
        public Category? Category { get; set; }
    }
}
