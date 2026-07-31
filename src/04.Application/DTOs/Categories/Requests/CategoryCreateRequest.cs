using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Categories.Requests
{
    /// <summary>
    /// Data required to create a new category.
    /// </summary>
    public class CategoryCreateRequest
    {
        /// <summary>Display name of the category. Must be unique among active categories.</summary>
        public string Name { get; set; } = default!;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Whether this category applies to income or expense transactions.</summary>
        public TransactionType Type { get; set; }
    }
}
