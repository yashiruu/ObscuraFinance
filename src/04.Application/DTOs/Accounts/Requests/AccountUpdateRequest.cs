using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Requests
{
    /// <summary>
    /// Data required to update an existing account.
    /// </summary>
    public class AccountUpdateRequest
    {
        /// <summary>Display name of the account. Must be unique among active accounts.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>The kind of account.</summary>
        public AccountType Type { get; set; }

        /// <summary>ISO currency code. Default: "IDR".</summary>
        public string Currency { get; set; } = "IDR";

        /// <summary>Whether the account is active and available for new transactions.</summary>
        public bool IsActive { get; set; } = true;
    }
}
