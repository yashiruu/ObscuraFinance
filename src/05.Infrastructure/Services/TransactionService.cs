using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Infrastructure.Persistence;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var transactions = await _context.Transactions
                .OrderByDescending(t => t.Date)
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

            return transactions;
        }
        public async Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .Where(t => t.Id == id)
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

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with '{id}' was not found.");
            }

            return transaction;
        }
        public async Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Where(a => a.Id == request.AccountId)
                .Select(a => new
                {
                    a.Id,
                    a.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            var category = await _context.Categories
                .Where(c => c.Id == request.CategoryId)
                .Select(c => new
                {
                    c.Id,
                    c.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with `{request.AccountId}` was not found");
            }

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with `{request.CategoryId}` was not found");
            }

            var transaction = new Transaction
            {
                Date = request.Date,
                Name = request.Name,
                Amount = request.Amount,
                Type = request.Type,
                CategoryId = request.CategoryId,
                AccountId = request.AccountId
            };

            await _context.Transactions.AddAsync(transaction, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var accountName = await _context.Accounts
                .Where(a => a.Id == request.AccountId)
                .Select(a => a.Name)
                .FirstOrDefaultAsync(cancellationToken);

            var categoryName = await _context.Categories
                .Where(c => c.Id == request.CategoryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync(cancellationToken);

            return new TransactionDetailResponse
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Name = transaction.Name,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                CategoryName = category.Name,
                AccountId = transaction.AccountId,
                AccountName = account.Name,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = transaction.CreatedBy
            };
        }
        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            var accountExists = await _context.Accounts
                .AnyAsync(a => a.Id == request.AccountId, cancellationToken);

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!accountExists)
            {
                throw new KeyNotFoundException($"Account with `{request.AccountId}` was not found");
            }

            if (!categoryExists)
            {
                throw new KeyNotFoundException($"Category with `{request.CategoryId}` was not found");
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with '{id}' was not found.");
            }

            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.AccountId = request.AccountId;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been deleted");
            }

            transaction.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted, cancellationToken);

            if (transaction == null)
            {
                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been restored");
            }

            transaction.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
