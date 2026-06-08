using Obscura.FinanceTracker.Base.Entities;

namespace Obscura.FinanceTracker.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Name { get; set; } = String.Empty;
        public Decimal Amount { get; set; }
        public string Type { get; set; } = String.Empty;
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }

        // Navigation Property
        public Account? Account { get; set; }
        public Category? Category { get; set; }
    }
}
