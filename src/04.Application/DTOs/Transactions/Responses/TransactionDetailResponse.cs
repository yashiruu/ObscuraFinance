using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Responses
{
    /// <summary>
    /// Detailed transaction information, including account/category names and audit fields.
    /// </summary>
    public class TransactionDetailResponse
    {
        /// <summary>Unique identifier of the transaction.</summary>
        public Guid Id { get; set; }

        /// <summary>Date the transaction occurred.</summary>
        public DateTime Date { get; set; }

        /// <summary>Short description of the transaction.</summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>Transaction amount.</summary>
        public decimal Amount { get; set; }

        /// <summary>Whether this transaction is income or expense.</summary>
        public TransactionType Type { get; set; }

        /// <summary>Id of the associated category.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Name of the associated category.</summary>
        public string CategoryName { get; set; } = String.Empty;

        /// <summary>Id of the associated account.</summary>
        public Guid AccountId { get; set; }

        /// <summary>Name of the associated account.</summary>
        public string AccountName { get; set; } = String.Empty;

        /// <summary>UTC timestamp at which the transaction was created.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Id of the user who created the transaction, if known.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>UTC timestamp of the last update, if any.</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Id of the user who last updated the transaction, if known.</summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>Whether the transaction is soft-deleted.</summary>
        public bool IsDeleted { get; set; } = false;
    }
}
