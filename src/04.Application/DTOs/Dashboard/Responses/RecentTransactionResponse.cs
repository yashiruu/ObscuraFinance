using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    /// <summary>
    /// A single recent transaction entry shown on the dashboard.
    /// </summary>
    public class RecentTransactionResponse
    {
        /// <summary>Unique identifier of the transaction.</summary>
        public Guid Id { get; set; }

        /// <summary>Date the transaction occurred.</summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>Short description of the transaction.</summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>Whether this transaction is income or expense.</summary>
        public TransactionType Type { get; set; }

        /// <summary>Name of the associated category.</summary>
        public string Category { get; set; } = String.Empty;

        /// <summary>Name of the associated account.</summary>
        public string Account { get; set; } = String.Empty;

        /// <summary>Transaction amount.</summary>
        public decimal Amount { get; set; }
    }
}
