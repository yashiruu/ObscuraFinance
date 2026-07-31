using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Client.Constants;
using Obscura.FinanceTracker.Shared.Models;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    /// <summary>
    /// Typed HTTP client for the Transaction API.
    /// </summary>
    public class TransactionClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionClient"/> class.
        /// </summary>
        public TransactionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a paged list of active transactions.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based). Default: 1.</param>
        /// <param name="pageSize">The number of items per page (max 50). Default: 10.</param>
        public async Task<PagedResult<TransactionListResponse>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Transactions}?pageNumber={pageNumber}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<TransactionListResponse>>>();

            return result?.Data ?? new PagedResult<TransactionListResponse>();
        }

        /// <summary>
        /// Retrieves a single transaction by id, or <see langword="null"/> if it does not exist.
        /// </summary>
        /// <param name="id">The transaction id.</param>
        public async Task<TransactionDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Transactions}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TransactionDetailResponse>>();

            return result?.Data;
        }

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <param name="request">The transaction data to create.</param>
        public async Task CreateAsync(TransactionCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Transactions}", request);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to update.</param>
        /// <param name="request">The updated transaction data.</param>
        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Transactions}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Soft deletes a transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to delete.</param>
        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Transactions}/{id}");

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Restores a previously soft-deleted transaction.
        /// </summary>
        /// <param name="id">The id of the transaction to restore.</param>
        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Transactions}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
