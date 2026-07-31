using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Responses
{
    /// <summary>
    /// Summarized account information returned by list endpoints.
    /// </summary>
    public class AccountListResponse
    {
        /// <summary>Unique identifier of the account.</summary>
        public Guid Id { get; set; }

        /// <summary>Display name of the account.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Current balance.</summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>The kind of account.</summary>
        public AccountType Type { get; set; }

        /// <summary>Whether the account is active and available for new transactions.</summary>
        public bool IsActive { get; set; }
    }
}
