using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Responses
{
    public class AccountDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal CurrentBalance { get; set; }
        public decimal InitialBalance { get; set; }

        public string Currency { get; set; } = string.Empty;
        public AccountType Type { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
