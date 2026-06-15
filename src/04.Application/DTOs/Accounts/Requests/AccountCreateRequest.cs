using Obscura.FinanceTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Obscura.FinanceTracker.Application.DTOs.Accounts.Requests
{
    public class AccountCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal InitialBalance { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; } = "IDR";

        [Required]
        public AccountType Type { get; set; }
    }
}