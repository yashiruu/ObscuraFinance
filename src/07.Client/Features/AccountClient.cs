using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Client.Constants;
using Obscura.FinanceTracker.Shared.Models;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    /// <summary>
    /// Typed HTTP client for the Account API.
    /// </summary>
    public class AccountClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountClient"/> class.
        /// </summary>
        public AccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a paged list of active accounts.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based). Default: 1.</param>
        /// <param name="pageSize">The number of items per page (max 50). Default: 10.</param>
        public async Task<PagedResult<AccountListResponse>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Accounts}?pageNumber={pageNumber}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<AccountListResponse>>>();

            return result?.Data ?? new PagedResult<AccountListResponse>();
        }

        /// <summary>
        /// Retrieves a single account by id, or <see langword="null"/> if it does not exist.
        /// </summary>
        /// <param name="id">The account id.</param>
        public async Task<AccountDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Accounts}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AccountDetailResponse>>();

            return result?.Data;
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="request">The account data to create.</param>
        public async Task CreateAsync(AccountCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Accounts}", request);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates an existing account.
        /// </summary>
        /// <param name="id">The id of the account to update.</param>
        /// <param name="request">The updated account data.</param>
        public async Task UpdateAsync(Guid id, AccountUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Accounts}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Soft deletes an account.
        /// </summary>
        /// <param name="id">The id of the account to delete.</param>
        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Accounts}/{id}");

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Restores a previously soft-deleted account.
        /// </summary>
        /// <param name="id">The id of the account to restore.</param>
        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Accounts}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
