using Obscura.FinanceTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Requests
{
    public class AccountCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; } = "IDR";
        public AccountType Type { get; set; }
    }
}