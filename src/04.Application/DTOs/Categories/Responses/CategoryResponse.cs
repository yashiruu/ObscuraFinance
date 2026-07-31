using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.DTOs.Categories.Responses
{
    /// <summary>
    /// Category information returned by category endpoints.
    /// </summary>
    public class CategoryResponse
    {
        /// <summary>Unique identifier of the category.</summary>
        public Guid Id { get; set; }

        /// <summary>Display name of the category.</summary>
        public string Name { get; set; } = default!;

        /// <summary>Optional free-text description.</summary>
        public string? Description { get; set; }

        /// <summary>Whether this category applies to income or expense transactions.</summary>
        public TransactionType Type { get; set; }
    }
}
