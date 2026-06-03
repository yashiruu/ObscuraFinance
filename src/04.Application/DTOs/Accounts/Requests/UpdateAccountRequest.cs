using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.Accounts.DTOs
{
    public class UpdateAccountRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AccountType Type { get; set; }

        public bool IsActive { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
    }
}