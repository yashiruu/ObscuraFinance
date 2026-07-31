using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Requests
{
    /// <summary>
    /// Data required to create a new transaction.
    /// </summary>
    public class TransactionCreateRequest
    {
        /// <summary>Date the transaction occurred.</summary>
        public DateTime Date { get; set; }

        /// <summary>Short description of the transaction.</summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>Transaction amount. Must be positive; direction is determined by <see cref="Type"/>.</summary>
        public decimal Amount { get; set; }

        /// <summary>Whether this transaction is income or expense.</summary>
        public TransactionType Type { get; set; }

        /// <summary>Id of the category this transaction belongs to. Must reference an existing category.</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Id of the account this transaction is recorded against. Must reference an existing account.</summary>
        public Guid AccountId { get; set; }
    }
}
