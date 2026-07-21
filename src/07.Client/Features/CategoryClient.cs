using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Client.Constants;
using System.Net;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    public class CategoryClient
    {
        private readonly HttpClient _httpClient;

        public CategoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoryResponse>>>();

            return result?.Data ?? [];
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryResponse>>();

            return result?.Data;
        }

        public async Task<List<CategoryResponse>> GetByTypeAsync(int type)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/type/{type}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoryResponse>>>();

            return result?.Data ?? [];
        }

        public async Task<List<CategoryResponse>> GetDeletedAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/deleted");
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoryResponse>>>();

            return result?.Data ?? [];
        }

        public async Task CreateAsync(CategoryCreateRequest createCategoryRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Categories}", createCategoryRequest);
            
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, CategoryUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Categories}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Categories}/{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Categories}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
