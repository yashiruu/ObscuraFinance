using Obscura.FinanceTracker.Base.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Domain.Entities
{
    /// <summary>
    /// A financial account (e.g. bank, e-wallet, cash) that holds a balance and owns transactions.
    /// </summary>
    public class Account : BaseEntity
    {
        /// <summary>Display name of the account. Must be unique among active accounts.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Balance the account was opened with.</summary>
        public decimal InitialBalance { get; set; } = Decimal.Zero;

        /// <summary>Current balance, updated as transactions are recorded.</summary>
        public decimal CurrentBalance { get; set; } = Decimal.Zero;

        /// <summary>ISO currency code. Default: "IDR".</summary>
        public string Currency { get; set; } = "IDR";

        /// <summary>The kind of account.</summary>
        public AccountType Type { get; set; } = AccountType.Bank;

        /// <summary>Whether the account is active and available for new transactions.</summary>
        public bool IsActive { get; set; } = true;
    }
}
