using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountListResponse>>> GetAll()
        {
            var accounts = await _dbContext.Accounts
                .Where(a => !a.IsDeleted)
                .Select(a => new AccountListResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    CurrentBalance = a.CurrentBalance,
                    Type = a.Type,
                    IsActive = a.IsActive
                })
                .ToListAsync();

            return Ok(accounts);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountDetailResponse>> GetById(Guid id)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.Id == id && !a.IsDeleted)
                .Select(a => new AccountDetailResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    CurrentBalance = a.CurrentBalance,
                    InitialBalance = a.InitialBalance,
                    Currency = a.Currency,
                    Type = a.Type,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (account == null) return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAccountRequest request)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                Currency = request.Currency,
                Type = request.Type,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.Empty
            };

            var exists = await _dbContext.Accounts.AnyAsync(a => a.Name == request.Name);

            if (exists) return BadRequest(new { errors = new { Name = new[] { "Account name already exists." } } });

            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();

            var response = new AccountDetailResponse
            {
                Id = account.Id,
                Name = account.Name,
                Description = account.Description,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance,
                Currency = account.Currency,
                Type = account.Type,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, UpdateAccountRequest request)
        {
            var exists = await _dbContext.Accounts.AnyAsync(a => a.Id != id && a.Name == request.Name && !a.IsDeleted);

            if (exists) return BadRequest(new { errors = new { Name = new[] { "Account name already exists."  } } });

            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a=> a.Id == id && !a.IsDeleted);

            if (account == null) return NotFound();

            account.Name = request.Name;
            account.Description = request.Description;
            account.IsActive = request.IsActive;
            account.UpdatedAt = DateTime.UtcNow;
            account.UpdatedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (account == null) return NotFound();

            account.IsDeleted = true;
            account.DeletedAt = DateTime.UtcNow;
            account.DeletedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted);

            if (account == null) return NotFound();

            account.IsDeleted = false;
            account.DeletedAt = null;
            account.DeletedBy = null;

            account.UpdatedAt = DateTime.UtcNow;
            account.UpdatedBy = Guid.Empty;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
