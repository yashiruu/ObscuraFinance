namespace Obscura.FinanceTracker.Domain.Enums
{
    /// <summary>
    /// The direction of a transaction relative to an account's balance.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>Money coming into the account.</summary>
        Income = 1,

        /// <summary>Money going out of the account.</summary>
        Expense = 2
    }
}
