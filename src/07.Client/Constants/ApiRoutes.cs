namespace Obscura.FinanceTracker.Client.Constants
{
    /// <summary>
    /// Relative base routes for the Web API endpoints, shared by all typed clients.
    /// </summary>
    public static class ApiRoutes
    {
        private const string BaseApi = "api/v1";

        /// <summary>Base route for the Category API.</summary>
        public const string Categories = $"{BaseApi}/category";

        /// <summary>Base route for the Account API.</summary>
        public const string Accounts = $"{BaseApi}/account";

        /// <summary>Base route for the Transaction API.</summary>
        public const string Transactions = $"{BaseApi}/transaction";

        /// <summary>Base route for the Dashboard API.</summary>
        public const string Dashboard = $"{BaseApi}/dashboard";
    }
}
