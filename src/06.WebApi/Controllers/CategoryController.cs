using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Categories.DTOs;
using Obscura.FinanceTracker.Application.Categories.Requests;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        // Inject the AppDbContext to interact with the database
        private readonly AppDbContext _dbContext;

        // Constructor to initialize the AppDbContext
        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll()
        {
            var categories = await _dbContext.Categories
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = (int)c.Type
                })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetById(Guid id)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == id && c.IsDeleted == false)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = (int)c.Type
                })
                .FirstOrDefaultAsync();

            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet("type/{type:int}")]
        public async Task<ActionResult<List<CategoryDto>>> GetByType(int type)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.Type == (TransactionType)type && !c.IsDeleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = (int)c.Type
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<List<CategoryDto>>> GetDeleted()
        {
            var categories = await _dbContext.Categories
                .Where(c => c.IsDeleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = (int)c.Type
                })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCategoryRequest request)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Type = (TransactionType)request.Type,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty
            };

            _dbContext.Categories.Add(category);
            
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = (int)category.Type
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateCategoryRequest request)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null) return NotFound();

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = (TransactionType)request.Type;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null) return NotFound();

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;
            category.DeletedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            if (category == null) return NotFound();

            category.IsDeleted = false;
            category.DeletedAt = null;
            category.DeletedBy = null;

            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
