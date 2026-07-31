using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Requests
{
    /// <summary>
    /// Data required to create a new account.
    /// </summary>
    public class AccountCreateRequest
    {
        /// <summary>Display name of the account. Must be unique among active accounts.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Balance the account is opened with.</summary>
        public decimal InitialBalance { get; set; }

        /// <summary>ISO currency code. Default: "IDR".</summary>
        public string Currency { get; set; } = "IDR";

        /// <summary>The kind of account.</summary>
        public AccountType Type { get; set; }
    }
}
