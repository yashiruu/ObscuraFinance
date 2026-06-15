using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TransactionController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionListResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions
                .Where(t => !t.IsDeleted)
                .Select(t => new TransactionListResponse
                {
                    Id = t.Id,
                    Date = t.Date,
                    Name = t.Name,
                    Amount = t.Amount,
                    Type = t.Type,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category!.Name,
                    AccountId = t.AccountId,
                    AccountName = t.Account!.Name
                })
                .ToListAsync(cancellationToken);

            return Ok(transaction);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDetailResponse>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions
                .Where(t => t.Id == id && !t.IsDeleted)
                .Select(t => new TransactionDetailResponse
                {
                    Id = t.Id,
                    Date = t.Date,
                    Name = t.Name,
                    Amount = t.Amount,
                    Type = t.Type,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category!.Name,
                    AccountId = t.AccountId,
                    AccountName = t.Account!.Name,
                    CreatedAt = t.CreatedAt,
                    CreatedBy = t.CreatedBy,
                    UpdatedAt = t.UpdatedAt,
                    UpdatedBy = t.UpdatedBy,
                    IsDeleted = t.IsDeleted
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Date = request.Date,
                Name = request.Name,
                Amount = request.Amount,
                Type = request.Type,
                CategoryId = request.CategoryId,
                AccountId = request.AccountId
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = new TransactionDetailResponse
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Name = transaction.Name,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                AccountId = transaction.AccountId,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = Guid.Empty
            };

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);

            if (transaction == null) return NotFound();

            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.AccountId = request.AccountId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete (Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);

            if (transaction == null) return NotFound();

            transaction.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/restore")]
        public async Task<ActionResult> Restore(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted, cancellationToken);

            if (transaction == null) return NotFound();

            transaction.IsDeleted = false;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
