namespace Obscura.FinanceTracker.Application.DTOs.Dashboard.Responses
{
    /// <summary>
    /// Per-account balance summary shown on the dashboard.
    /// </summary>
    public class AccountSummaryResponse
    {
        /// <summary>Unique identifier of the account.</summary>
        public Guid Id { get; set; }

        /// <summary>Display name of the account.</summary>
        public string AccountName { get; set; } = String.Empty;

        /// <summary>Current balance.</summary>
        public decimal Balance { get; set; }
    }
}
