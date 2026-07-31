using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Responses
{
    /// <summary>
    /// Detailed account information returned by single-account endpoints.
    /// </summary>
    public class AccountDetailResponse
    {
        /// <summary>Unique identifier of the account.</summary>
        public Guid Id { get; set; }

        /// <summary>Display name of the account.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Current balance.</summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>Balance the account was opened with.</summary>
        public decimal InitialBalance { get; set; }

        /// <summary>ISO currency code.</summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>The kind of account.</summary>
        public AccountType Type { get; set; }

        /// <summary>Whether the account is active and available for new transactions.</summary>
        public bool IsActive { get; set; }

        /// <summary>UTC timestamp at which the account was created.</summary>
        public DateTime CreatedAt { get; set; }
    }
}
