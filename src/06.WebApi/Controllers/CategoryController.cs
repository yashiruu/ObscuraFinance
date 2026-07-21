using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    /// <summary>
    /// Manages financial transaction categories within the system.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all active categories.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of category summaries.</returns>
        /// <response code="200">Returns the list of categories successfully.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Categories retrieved successfully"));
        }

        /// <summary>
        /// Retrieves a specific category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Detailed information about the requested category.</returns>
        /// <response code="200">Returns the requested category.</response>
        /// <response code="404">If the category with the specified ID does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (category == null) return NotFound();

            return Ok(ApiResponse<CategoryResponse>.SuccessResponse(category, "Category retrieved successfully"));
        }

        /// <summary>
        /// Retrieves all soft-deleted categories.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of deleted category summaries.</returns>
        /// <response code="200">Returns the list of deleted categories successfully.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetDeleted(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetDeletedAsync(cancellationToken);

            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Deleted categories retrieved successfully"));
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="request">The data required to create a new category.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The newly created category details.</returns>
        /// <response code="201">Returns the newly created category.</response>
        /// <response code="400">If the request data is invalid or if a category with the same name already exists.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, ApiResponse<CategoryResponse>.SuccessResponse(category, "Category created successfully"));
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The unique identifier of the category to update.</param>
        /// <param name="request">The updated category data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the category was updated successfully.</response>
        /// <response code="400">If the request data is invalid or if the new name conflicts with an existing category.</response>
        /// <response code="404">If the category with the specified ID does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            await _categoryService.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Soft deletes a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the category was deleted successfully.</response>
        /// <response code="404">If the category with the specified ID does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Restores a previously soft-deleted category.
        /// </summary>
        /// <param name="id">The unique identifier of the category to restore.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">If the category was restored successfully.</response>
        /// <response code="400">If restoring the category causes a name duplication.</response>
        /// <response code="404">If the category with the specified ID does not exist.</response>
        /// <response code="500">If an unexpected internal server error occurs.</response>
        [HttpPatch("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.RestoreAsync(id, cancellationToken);

            return NoContent();
        }
    }
}