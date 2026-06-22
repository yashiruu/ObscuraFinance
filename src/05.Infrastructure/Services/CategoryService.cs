using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all categories");

            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                "Retrieved {Count} categories",
                categories.Count);

            return categories;
        }

        public async Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving categories by type. Type: {CategoryType}",
                type);

            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Where(c => c.Type == type)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                "Retrieved {Count} categories for type {CategoryType}",
                categories.Count,
                type);

            return categories;
        }

        public async Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving deleted categories");

            var categories = await _context.Categories
                .IgnoreQueryFilters()
                .OrderBy(c => c.Name)
                .Where(c => c.IsDeleted)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                "Retrieved {Count} deleted categories",
                categories.Count);

            return categories;
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving category. CategoryId: {CategoryId}",
                id);

            var category = await _context.Categories
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    id);

                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            _logger.LogInformation(
                "Category retrieved successfully. CategoryId: {CategoryId}",
                id);

            return category;
        }

        public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating category. CategoryName: {CategoryName}",
                request.Name);

            var exists = await _context.Categories
                .IgnoreQueryFilters()
                .AnyAsync(c => c.Name == request.Name && c.Type == (TransactionType)request.Type, cancellationToken);

            if (exists)
            {
                _logger.LogWarning(
                    "Category already exists. CategoryName: {CategoryName}",
                    request.Name);

                throw new ValidationException(
                    $"Category with '{request.Name}' already exists.");
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                Type = (TransactionType)request.Type
            };

            await _context.Categories.AddAsync(category, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Category created successfully. CategoryId: {CategoryId}",
                category.Id);

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
            _logger.LogInformation(
                "Updating category. CategoryId: {CategoryId}",
                id);

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null) 
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    id);

                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            var exists = await _context.Categories
                .IgnoreQueryFilters()
                .AnyAsync(c => c.Name == request.Name && c.Type == (TransactionType)request.Type, cancellationToken);

            if (exists)
            {
                _logger.LogWarning(
                    "Category already exists. CategoryName: {CategoryName}",
                    request.Name);

                throw new ValidationException(
                    $"Category with '{request.Name}' already exists.");
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = (TransactionType)request.Type;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Category updated successfully. CategoryId: {CategoryId}",
                id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Deleting category. CategoryId: {CategoryId}",
                id);

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    id);

                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            category.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Category deleted successfully. CategoryId: {CategoryId}",
                id);
        }

        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Restoring category. CategoryId: {CategoryId}",
                id);

            var category = await _context.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    id);

                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            category.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Category restored successfully. CategoryId: {CategoryId}",
                id);
        }
    }
}
