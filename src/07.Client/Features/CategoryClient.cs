using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Client.Constants;
using Obscura.FinanceTracker.Shared.Models;
using System.Net;
using System.Net.Http.Json;

namespace Obscura.FinanceTracker.Client.Features
{
    /// <summary>
    /// Typed HTTP client for the Category API.
    /// </summary>
    public class CategoryClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryClient"/> class.
        /// </summary>
        public CategoryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a paged list of active categories.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based). Default: 1.</param>
        /// <param name="pageSize">The number of items per page (max 50). Default: 10.</param>
        public async Task<PagedResult<CategoryResponse>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}?pageNumber={pageNumber}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<CategoryResponse>>>();

            return result?.Data ?? new PagedResult<CategoryResponse>();
        }

        /// <summary>
        /// Retrieves a single category by id, or <see langword="null"/> if it does not exist.
        /// </summary>
        /// <param name="id">The category id.</param>
        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryResponse>>();

            return result?.Data;
        }

        /// <summary>
        /// Retrieves all active categories of the given transaction type.
        /// </summary>
        /// <param name="type">The transaction type to filter by (numeric value of <c>TransactionType</c>).</param>
        public async Task<List<CategoryResponse>> GetByTypeAsync(int type)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/type/{type}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<CategoryResponse>>>();

            return result?.Data ?? [];
        }

        /// <summary>
        /// Retrieves a paged list of soft-deleted categories.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based). Default: 1.</param>
        /// <param name="pageSize">The number of items per page (max 50). Default: 10.</param>
        public async Task<PagedResult<CategoryResponse>> GetDeletedAsync(int pageNumber = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"{ApiRoutes.Categories}/deleted?pageNumber={pageNumber}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<CategoryResponse>>>();

            return result?.Data ?? new PagedResult<CategoryResponse>();
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createCategoryRequest">The category data to create.</param>
        public async Task CreateAsync(CategoryCreateRequest createCategoryRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiRoutes.Categories}", createCategoryRequest);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The id of the category to update.</param>
        /// <param name="request">The updated category data.</param>
        public async Task UpdateAsync(Guid id, CategoryUpdateRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiRoutes.Categories}/{id}", request);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Soft deletes a category.
        /// </summary>
        /// <param name="id">The id of the category to delete.</param>
        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiRoutes.Categories}/{id}");

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Restores a previously soft-deleted category.
        /// </summary>
        /// <param name="id">The id of the category to restore.</param>
        public async Task RestoreAsync(Guid id)
        {
            var response = await _httpClient.PatchAsync($"{ApiRoutes.Categories}/{id}/restore", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
