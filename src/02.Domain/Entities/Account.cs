using Obscura.FinanceTracker.Base.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal InitialBalance { get; set; } = Decimal.Zero;
        public decimal CurrentBalance { get; set; } = Decimal.Zero;
        public string Currency { get; set; } = "IDR";
        public AccountType Type { get; set; } = AccountType.Bank;
        public bool IsActive { get; set; } = true;
    }
}
