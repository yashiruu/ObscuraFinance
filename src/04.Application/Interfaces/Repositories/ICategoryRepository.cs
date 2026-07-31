using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    /// <summary>
    /// Data access contract for <see cref="Category"/>, extending the generic repository with category-specific queries.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Retrieves all active categories of the given transaction type, ordered by name.
        /// </summary>
        /// <param name="type">The transaction type to filter by.</param>
        Task<IReadOnlyList<Category>> GetAllByTypeAsync(TransactionType type);

        /// <summary>
        /// Retrieves a page of soft-deleted categories along with the total count.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A tuple containing the collection of deleted categories and the total record count.</returns>
        Task<(IReadOnlyList<Category> Items, int TotalCount)> GetAllDeletedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Checks whether a category name is already in use.
        /// </summary>
        /// <param name="name">The category name to check.</param>
        /// <param name="excludeId">An optional category id to exclude from the check (used when updating).</param>
        Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null);
    }
}
