using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(AppDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving transactions");

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

            _logger.LogInformation(
                "Retrieved {Count} transactions",
                transactions.Count);

            return transactions;
        }
        public async Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving transaction. TransactionId: {TransactionId}",
                id);

            var transaction = await _context.Transactions
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
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found.");
            }

            _logger.LogInformation(
                "Transaction retrieved successfully. TransactionId: {TransactionId}",
                id);

            return transaction;
        }
        public async Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Creating transaction. TransactionName: {TransactionName}",
                request.Name);

            var accountExists = await _context.Accounts
                .Where(a => a.Id == request.AccountId)
                .Select(a => new {a.Name})  // get only the Name property to reduce data transfer for response
                .FirstOrDefaultAsync(cancellationToken);

            var categoryExists = await _context.Categories
                .Where(c => c.Id == request.CategoryId)
                .Select(c => new {c.Name})  // get only the Name property to reduce data transfer for response
                .FirstOrDefaultAsync(cancellationToken);

            if (accountExists == null)
            {
                _logger.LogWarning(
                    "Account not found. AccountId: {AccountId}",
                    request.AccountId);

                throw new KeyNotFoundException($"Account with `{request.AccountId}` was not found");
            }

            if (categoryExists == null)
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    request.CategoryId);

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

            _logger.LogInformation(
                "Transaction created successfully. TransactionId: {TransactionId}",
                transaction.Id);

            return new TransactionDetailResponse
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Name = transaction.Name,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                CategoryName = categoryExists.Name,
                AccountId = transaction.AccountId,
                AccountName = accountExists.Name,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = transaction.CreatedBy
            };
        }
        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Updating transaction. TransactionId: {TransactionId}",
                id);

            var accountExists = await _context.Accounts
                .AnyAsync(a => a.Id == request.AccountId, cancellationToken);

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!accountExists)
            {
                _logger.LogWarning(
                    "Account not found. AccountId: {AccountId}",
                    request.AccountId);

                throw new KeyNotFoundException($"Account with `{request.AccountId}` was not found");
            }

            if (!categoryExists)
            {
                _logger.LogWarning(
                    "Category not found. CategoryId: {CategoryId}",
                    request.CategoryId);

                throw new KeyNotFoundException($"Category with `{request.CategoryId}` was not found");
            }

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found.");
            }

            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.AccountId = request.AccountId;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction updated successfully. TransactionId: {TransactionId}",
                id);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Deleting transaction. TransactionId: {TransactionId}",
                id);

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been deleted");
            }

            transaction.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction deleted successfully. TransactionId: {TransactionId}",
                id);
        }
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Restoring transaction. TransactionId: {TransactionId}",
                id);

            var transaction = await _context.Transactions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted, cancellationToken);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been restored");
            }

            transaction.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction restored successfully. TransactionId: {TransactionId}",
                id);
        }
    }
}
