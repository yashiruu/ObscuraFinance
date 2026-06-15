using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
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
        public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.IsDeleted == false)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == id && c.IsDeleted == false)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet("{type:int}")]
        public async Task<ActionResult<List<CategoryDto>>> GetByType(int type, CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.Type == (TransactionType)type && !c.IsDeleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);

            return Ok(categories);
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<List<CategoryDto>>> GetDeleted(CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories
                .Where(c => c.IsDeleted)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Type = c.Type
                })
                .ToListAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Type = (TransactionType)request.Type,
                IsDeleted = false
            };

            _dbContext.Categories.Add(category);
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Type = category.Type
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

            if (category == null) return NotFound();

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = (TransactionType)request.Type;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

            if (category == null) return NotFound();

            category.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted, cancellationToken);

            if (category == null) return NotFound();

            category.IsDeleted = false;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
