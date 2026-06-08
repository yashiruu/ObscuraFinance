using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using System.Net;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Categories
{
    public class CategoryClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/category";

        public CategoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? [];
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        public async Task<List<CategoryDto>> GetByTypeAsync(int type)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/type/{type}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? [];
        }

        public async Task<List<CategoryDto>> GetDeletedAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/deleted");
            
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? [];
        }

        public async Task CreateAsync(CreateCategoryRequest createCategoryRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}", createCategoryRequest);
            
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryRequest request)
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
