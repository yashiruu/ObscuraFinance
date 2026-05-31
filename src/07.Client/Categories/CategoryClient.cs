using Obscura.FinanceTracker.Application.Categories.DTOs;
using Obscura.FinanceTracker.Application.Categories.Requests;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Categories
{
    public class CategoryClient
    {
        private readonly HttpClient _httpClient;

        public CategoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? [];
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>("api/categories/" + id) ?? new CategoryDto();
        }

        public async Task CreateAsync(CreateCategoryRequest createCategoryRequest)
        {
            await _httpClient.PostAsJsonAsync("api/categories", createCategoryRequest);
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            await _httpClient.PutAsJsonAsync($"api/category/{id}", request);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"api/category/{id}");
        }
    }
}
