using Obscura.FinanceTracker.Base.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Domain.Entities
{
    /// <summary>
    /// A single income or expense movement recorded against an <see cref="Account"/> and <see cref="Category"/>.
    /// </summary>
    public class Transaction : BaseEntity
    {
        /// <summary>Date the transaction occurred.</summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>Short description of the transaction.</summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>Transaction amount. Always positive; direction is determined by <see cref="Type"/>.</summary>
        public decimal Amount { get; set; }

        /// <summary>Whether this transaction is income or expense.</summary>
        public TransactionType Type { get; set; }

        /// <summary>
        /// Gets or sets the foreign key for the associated Account entity.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>Foreign key of the associated <see cref="Category"/>.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the associated Account entity.
        /// This property represents the navigation property for the relationship between Transaction and Account.
        /// </summary>
        public Account? Account { get; set; }

        /// <summary>Navigation property for the associated <see cref="Category"/>.</summary>
        public Category? Category { get; set; }
    }
}
