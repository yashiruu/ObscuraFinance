using FluentValidation;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TransactionCreateRequest> _validator;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IUnitOfWork unitOfWork, IValidator<TransactionCreateRequest> validator, ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving transactions");

            var transactions = await _unitOfWork.Transactions.GetAllWithDetailAsync();

            _logger.LogInformation("Retrieved {Count} transactions", transactions.Count);

            return transactions.Select(transaction => new TransactionListResponse
            {
                Id = transaction.Id,
                Date = transaction.Date,
                Name = transaction.Name,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category!.Name,
                AccountId = transaction.AccountId,
                AccountName = transaction.Account!.Name
            });
        }

        public async Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Retrieving transaction. TransactionId: {TransactionId}",
                id);

            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found. TransactionId: {TransactionId}", id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found.");
            }

            _logger.LogInformation("Transaction retrieved successfully. TransactionId: {TransactionId}", id);

            return new TransactionDetailResponse
            {
                Id = id,
                Date = transaction.Date,
                Name = transaction.Name,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Name,
                AccountId = transaction.AccountId,
                AccountName = transaction.Name,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = transaction.CreatedBy,
                UpdatedAt = transaction.UpdatedAt,
                UpdatedBy = transaction?.UpdatedBy
            };
        }

        public async Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Creating transaction. TransactionName: {TransactionName}", request.Name);

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", request.AccountId);

                throw new KeyNotFoundException($"Account with `{request.AccountId}` was not found");
            }

            if (category == null)
            {
                _logger.LogWarning( "Category not found. CategoryId: {CategoryId}", request.CategoryId);
                
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

            await _unitOfWork.Transactions.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transaction created successfully. TransactionId: {TransactionId}", transaction.Id);

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
            _logger.LogInformation(
                "Updating transaction. TransactionId: {TransactionId}",
                id);

            var accountExists = await _unitOfWork.Transactions.ExistsAsync(a => a.Id == id);

            var categoryExists = await _unitOfWork.Transactions.ExistsAsync(c => c.Id == id);

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

            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

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

            _unitOfWork.Transactions.Update(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction updated successfully. TransactionId: {TransactionId}",
                id);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting transaction. TransactionId: {TransactionId}", id);

            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been deleted");
            }

            _unitOfWork.Transactions.Delete(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction deleted successfully. TransactionId: {TransactionId}",
                id);
        }

        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Restoring transaction. TransactionId: {TransactionId}",
                id);

            var transaction = await _unitOfWork.Transactions.GetByIdIncludingDeletedAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning(
                    "Transaction not found. TransactionId: {TransactionId}",
                    id);

                throw new KeyNotFoundException($"Transaction with '{id}' was not found or has been restored");
            }

            transaction.IsDeleted = false;

            _unitOfWork.Transactions.Update(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Transaction restored successfully. TransactionId: {TransactionId}",
                id);
        }
    }
}
