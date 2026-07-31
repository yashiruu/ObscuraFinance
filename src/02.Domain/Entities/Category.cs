using Obscura.FinanceTracker.Base.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Domain.Entities
{
    /// <summary>
    /// A category used to classify transactions (e.g. "Groceries", "Salary").
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>Display name of the category. Must be unique among active categories.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Whether this category applies to income or expense transactions.</summary>
        public TransactionType Type { get; set; }
    }
}
