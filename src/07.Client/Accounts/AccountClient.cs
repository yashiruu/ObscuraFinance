using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Accounts
{
    public class AccountClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/account";

        public AccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AccountListResponse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<AccountListResponse>>() ?? [];
        }

        public async Task<AccountDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<AccountDetailResponse>();
        }

        public async Task CreateAsync(CreateAccountRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", request);
            
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, UpdateAccountRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{BaseUrl}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
