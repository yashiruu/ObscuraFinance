using Obscura.FinanceTracker.Application.Categories.DTOs;
using Obscura.FinanceTracker.Application.Categories.Requests;
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
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>($"{BaseUrl}") ?? [];
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>($"{BaseUrl}/" + id);
        }

        public async Task<List<CategoryDto>> GetBytypeAsync(int type)
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>($"{BaseUrl}/type/" + type) ?? [];
        }

        public async Task<List<CategoryDto>> GetDeletedAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>($"{BaseUrl}/deleted") ?? [];
        }

        public async Task CreateAsync(CreateCategoryRequest createCategoryRequest)
        {
            await _httpClient.PostAsJsonAsync($"{BaseUrl}", createCategoryRequest);
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        }

        public async Task RestoreAsync(Guid id)
        {
            await _httpClient.PostAsync($"{BaseUrl}/{id}/restore", null);
        }
    }
}
