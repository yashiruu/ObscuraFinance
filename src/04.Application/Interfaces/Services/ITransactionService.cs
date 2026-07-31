using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.Application.Interfaces.Services
{
    /// <summary>
    /// Application service contract for managing financial transactions.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Retrieves a paged list of active transactions.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A paged result containing transaction data and navigation metadata.</returns>
        Task<PagedResult<TransactionListResponse>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single transaction, including account and category details, by its unique identifier.
        /// </summary>
        /// <param name="id">The transaction id.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The transaction does not exist.</exception>
        Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <param name="request">The transaction data to create.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The referenced account or category does not exist.</exception>
        Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to update.</param>
        /// <param name="request">The updated transaction data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The transaction, account, or category does not exist.</exception>
        Task UpdateAsync(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Soft deletes a transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to delete.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The transaction does not exist.</exception>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Restores a previously soft-deleted transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to restore.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The transaction does not exist.</exception>
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
