using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all categories");

            var categories = await _unitOfWork.Categories.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} categories", categories.Count);

            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            });
        }

        public async Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving categories by type. Type: {CategoryType}", type);

            var categories = await _unitOfWork.Category.GetAllByTypeAsync(type);

            _logger.LogInformation("Retrieved {Count} categories for type {CategoryType}", categories.Count, type);

            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            });
        }

        public async Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving deleted categories");

            var categories = await _unitOfWork.Category.Get

            _logger.LogInformation(
                "Retrieved {Count} deleted categories",
                categories.Count);

            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            });
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);

                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            _logger.LogInformation("Category retrieved successfully. CategoryId: {CategoryId}", id);

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            };
        }

        public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating category. CategoryName: {CategoryName}", request.Name);

            var exists = await _unitOfWork.Categories.ExistsAsync(c => c.Name == request.Name);

            if (exists)
            {
                _logger.LogWarning("Category already exists. CategoryName: {CategoryName}", request.Name);

                throw new ValidationException($"Category with '{request.Name}' already exists.");
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                Type = (TransactionType)request.Type
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category created successfully. CategoryId: {CategoryId}", category.Id);

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            };
        }

        public async Task UpdateAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null) 
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);

                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            var exists = await _unitOfWork.Categories.ExistsAsync(c => c.Id == id);

            if (exists)
            {
                _logger.LogWarning("Category already exists. CategoryName: {CategoryName}", request.Name);

                throw new ValidationException($"Category with '{request.Name}' already exists.");
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = (TransactionType)request.Type;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category updated successfully. CategoryId: {CategoryId}", id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);

                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Category deleted successfully. CategoryId: {CategoryId}",
                id);
        }

        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdIncludingDeletedAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);

                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            category.IsDeleted = false;

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category restored successfully. CategoryId: {CategoryId}", id);
        }
    }
}
