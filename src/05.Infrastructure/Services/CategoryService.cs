using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
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
            
            return categories;
        }

        public async Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
        {
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

            return categories;
        }

        public async Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);

            return categories;
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return category;
        }

        public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                Type = (TransactionType)request.Type
            };

            await _context.AddAsync(category, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

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
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null) 
            {
                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = (TransactionType)request.Type;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with '{id}' was not found or Category has been deleted");
            }

            category.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with '{id}' was not found or Category has been restored");
            }

            category.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
