using Obscura.FinanceTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Obscura.FinanceTracker.Application.DTOs.Transactions.Requests
{
    public  class TransactionCreateRequest
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid AccountId { get; set; }
    }
}
