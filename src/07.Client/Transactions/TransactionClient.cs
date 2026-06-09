using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Transactions
{
    public class TransactionClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/transaction";

        public TransactionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TransactionListResponse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<TransactionListResponse>>() ?? [];
        }

        public async Task<TransactionDetailResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TransactionDetailResponse>();
        }

        public async Task CreateAsync(TransactionCreateRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request)
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
