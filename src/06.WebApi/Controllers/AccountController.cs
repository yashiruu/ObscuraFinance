using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
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
                    Type = a.Type.ToString(),
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
                    Type = a.Type.ToString(),
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (account == null) return NotFound();

            return Ok(account);
        }
    }
}
