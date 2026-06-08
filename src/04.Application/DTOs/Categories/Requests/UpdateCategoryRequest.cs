using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Categories.Requests
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public TransactionType Type { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdateBy { get; set; }
    }
}
