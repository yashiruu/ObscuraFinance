using Obscura.FinanceTracker.Base.Entities;

namespace Obscura.FinanceTracker.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
