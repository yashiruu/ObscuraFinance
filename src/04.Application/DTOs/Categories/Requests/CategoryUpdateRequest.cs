using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Categories.Requests
{
    public class CategoryUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public TransactionType Type { get; set; }
    }
}
