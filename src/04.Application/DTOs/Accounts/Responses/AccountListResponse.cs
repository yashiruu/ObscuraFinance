namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Responses
{
    public class AccountListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
