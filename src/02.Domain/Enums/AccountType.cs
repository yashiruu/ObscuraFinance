namespace Obscura.FinanceTracker.Domain.Enums
{
    /// <summary>
    /// The kind of financial account.
    /// </summary>
    public enum AccountType
    {
        /// <summary>A traditional bank account.</summary>
        Bank = 0,

        /// <summary>A digital wallet (e.g. GoPay, OVO).</summary>
        EWallet = 1,

        /// <summary>Physical cash on hand.</summary>
        Cash = 2,

        /// <summary>An investment account (e.g. stocks, mutual funds).</summary>
        Investment = 3,

        /// <summary>A credit card account.</summary>
        CreditCard = 4,
    }
}
