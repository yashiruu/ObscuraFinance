using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.Accounts.DTOs
{
    public class AccountUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccountType Type { get; set; }
        public string Currency { get; set; } = "IDR";
        public bool IsActive { get; set; } = true;
    }
}