using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.Application.Interfaces.Services
{
    /// <summary>
    /// Application service contract for managing financial transaction categories.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves a paged list of active categories.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A paged result containing category data and navigation metadata.</returns>
        Task<PagedResult<CategoryResponse>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all active categories of the given transaction type.
        /// </summary>
        /// <param name="type">The transaction type to filter by.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a paged list of soft-deleted categories.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A paged result containing deleted category data and navigation metadata.</returns>
        Task<PagedResult<CategoryResponse>> GetDeletedAsync(PagedRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a single category by its unique identifier.
        /// </summary>
        /// <param name="id">The category id.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The category does not exist.</exception>
        Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The category data to create.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">A category with the same name already exists.</exception>
        Task<CategoryResponse> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The id of the category to update.</param>
        /// <param name="request">The updated category data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The category does not exist.</exception>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">The new name conflicts with an existing category.</exception>
        Task UpdateAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Soft deletes a category.
        /// </summary>
        /// <param name="id">The id of the category to delete.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The category does not exist.</exception>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Restores a previously soft-deleted category.
        /// </summary>
        /// <param name="id">The id of the category to restore.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The category does not exist.</exception>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">Restoring would conflict with an existing category name.</exception>
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
