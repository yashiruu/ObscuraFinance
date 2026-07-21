using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Requests
{
    public class AccountUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccountType Type { get; set; }
        public string Currency { get; set; } = "IDR";
        public bool IsActive { get; set; } = true;
    }
}