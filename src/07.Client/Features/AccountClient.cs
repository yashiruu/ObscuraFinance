using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Client.Constants;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    public class AccountClient
    {
        private readonly HttpClient _httpClient;

        public AccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AccountListResponse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Accounts);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AccountListResponse>>>();

            return result?.Data ?? [];
        }

        public async Task<AccountDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Accounts}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AccountDetailResponse>>();

            return result?.Data;
        }

        public async Task CreateAsync(AccountCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Accounts}", request);
            
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, AccountUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Accounts}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Accounts}/{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Accounts}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
