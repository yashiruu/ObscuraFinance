using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Shared.Exceptions;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.Application.Services
{
    /// <inheritdoc cref="ITransactionService"/>
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<TransactionCreateRequest> _createValidator;
        private readonly IValidator<TransactionUpdateRequest> _updateValidator;
        private readonly ILogger<TransactionService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        public TransactionService(IUnitOfWork unitOfWork, IValidator<TransactionCreateRequest> createValidator, IValidator<TransactionUpdateRequest> updateValidator, ILogger<TransactionService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<TransactionListResponse>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving transactions");

            var (transactions, totalCount) = await _unitOfWork.Transactions.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<TransactionListResponse>>(transactions);

            _logger.LogInformation("Retrieved {Count} transactions", transactions.Count);

            return new PagedResult<TransactionListResponse>
            {
                Items = dtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc/>
        public async Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving transaction. TransactionId: {TransactionId}", id);

            var transaction = await _unitOfWork.Transactions.GetByIdWithDetailAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found. TransactionId: {TransactionId}", id);
                throw NotFoundException.For<Transaction>(id);
            }

            _logger.LogInformation("Transaction retrieved successfully. TransactionId: {TransactionId}", id);

            return _mapper.Map<TransactionDetailResponse>(transaction);
        }

        /// <inheritdoc/>
        public async Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Creating transaction. TransactionName: {TransactionName}", request.Name);

            var accountExists = await _unitOfWork.Accounts.ExistsAsync(a => a.Id == request.AccountId);
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.Id == request.CategoryId);

            if (!accountExists)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", request.AccountId);
                throw NotFoundException.For<Account>(request.AccountId);
            }

            if (!categoryExists)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", request.CategoryId);
                throw NotFoundException.For<Category>(request.CategoryId);
            }

            var transaction = _mapper.Map<Transaction>(request);

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transaction created successfully. TransactionId: {TransactionId}", transaction.Id);

            return _mapper.Map<TransactionDetailResponse>(transaction);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Updating transaction. TransactionId: {TransactionId}", id);

            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found. TransactionId: {TransactionId}", id);
                throw NotFoundException.For<Transaction>(id);
            }

            var accountExists = await _unitOfWork.Accounts.ExistsAsync(a => a.Id == request.AccountId);
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.Id == request.CategoryId);

            if (!accountExists)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", request.AccountId);
                throw NotFoundException.For<Account>(request.AccountId);
            }

            if (!categoryExists)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", request.CategoryId);
                throw NotFoundException.For<Category>(request.CategoryId);
            }

            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.CategoryId = request.CategoryId;
            transaction.AccountId = request.AccountId;

            _unitOfWork.Transactions.Update(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transaction updated successfully. TransactionId: {TransactionId}", id);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting transaction. TransactionId: {TransactionId}", id);

            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found. TransactionId: {TransactionId}", id);
                throw NotFoundException.For<Transaction>(id);
            }

            _unitOfWork.Transactions.Delete(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transaction deleted successfully. TransactionId: {TransactionId}", id);
        }

        /// <inheritdoc/>
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring transaction. TransactionId: {TransactionId}", id);

            var transaction = await _unitOfWork.Transactions.GetByIdIncludingDeletedAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found. TransactionId: {TransactionId}", id);
                throw NotFoundException.For<Transaction>(id);
            }

            transaction.IsDeleted = false;

            _unitOfWork.Transactions.Update(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Transaction restored successfully. TransactionId: {TransactionId}", id);
        }
    }
}
