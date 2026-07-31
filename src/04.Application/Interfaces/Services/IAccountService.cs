using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.Application.Interfaces.Services
{
    /// <summary>
    /// Application service contract for managing financial accounts.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves a paged list of active accounts.
        /// </summary>
        /// <param name="request">Pagination parameters (settable: <c>PageNumber</c>, <c>PageSize</c>).</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A paged result containing account data and navigation metadata.</returns>
        Task<PagedResult<AccountListResponse>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single account by its unique identifier.
        /// </summary>
        /// <param name="id">The account id.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The account does not exist.</exception>
        Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="request">The account data to create.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">An account with the same name already exists.</exception>
        Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="id">The id of the account to update.</param>
        /// <param name="request">The updated account data.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The account does not exist.</exception>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">The new name conflicts with an existing account.</exception>
        Task UpdateAsync(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Soft deletes an account.
        /// </summary>
        /// <param name="id">The id of the account to delete.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The account does not exist.</exception>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Restores a previously soft-deleted account.
        /// </summary>
        /// <param name="id">The id of the account to restore.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.NotFoundException">The account does not exist.</exception>
        /// <exception cref="Obscura.FinanceTracker.Shared.Exceptions.BusinessException">Restoring would conflict with an existing account name.</exception>
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
