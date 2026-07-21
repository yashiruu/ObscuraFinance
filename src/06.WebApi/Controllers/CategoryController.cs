using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Categories retrieved successfully"));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (category == null) return NotFound();

            return Ok(ApiResponse<CategoryResponse>.SuccessResponse(category, "Category retrieved successfully"));
        }

        [HttpGet("type/{type:int}")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetByType(int type, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetByTypeAsync((TransactionType)type, cancellationToken);

            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Categories retrieved successfully"));
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetDeleted(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetDeletedAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Deleted categories retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, ApiResponse<CategoryResponse>.SuccessResponse(category, "Category created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
