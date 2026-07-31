using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Responses
{
    /// <summary>
    /// Summarized transaction information returned by list endpoints.
    /// </summary>
    public class TransactionListResponse
    {
        /// <summary>Unique identifier of the transaction.</summary>
        public Guid Id { get; set; }

        /// <summary>Date the transaction occurred.</summary>
        public DateTime Date { get; set; }

        /// <summary>Short description of the transaction.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Transaction amount.</summary>
        public decimal Amount { get; set; }

        /// <summary>Whether this transaction is income or expense.</summary>
        public TransactionType Type { get; set; }

        /// <summary>Id of the associated category.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Name of the associated category.</summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>Id of the associated account.</summary>
        public Guid AccountId { get; set; }

        /// <summary>Name of the associated account.</summary>
        public string AccountName { get; set; } = string.Empty;
    }
}
