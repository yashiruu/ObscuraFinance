using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Shared.Exceptions;
using Obscura.FinanceTracker.Shared.Models;

namespace Obscura.FinanceTracker.Application.Services
{
    /// <inheritdoc cref="IAccountService"/>
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AccountCreateRequest> _createValidator;
        private readonly IValidator<AccountUpdateRequest> _updateValidator;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        public AccountService(IUnitOfWork unitOfWork, IValidator<AccountCreateRequest> createValidator, IValidator<AccountUpdateRequest> updateValidator, ILogger<AccountService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<AccountListResponse>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all accounts");

            var (accounts, totalCount) = await _unitOfWork.Accounts.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

            _logger.LogInformation("Retrieved {Count} accounts", accounts.Count);

            return new PagedResult<AccountListResponse>
            {
                Items = _mapper.Map<IEnumerable<AccountListResponse>>(accounts),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
        /// <inheritdoc/>
        public async Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);
                throw NotFoundException.For<Account>(id);
            }

            _logger.LogInformation("Account retrieved successfully. AccountId: {AccountId}", id);

            return _mapper.Map<AccountDetailResponse>(account);
        }
        /// <inheritdoc/>
        public async Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Creating account. AccountName: {AccountName}", request.Name);

            var exists = await _unitOfWork.Accounts.IsNameTakenAsync(request.Name);

            if (exists)
            {
                _logger.LogWarning("Account's name already exists. AccountName: {AccountName}", request.Name);
                throw new BusinessException($"Account with '{request.Name}' already exists.");
            }

            var account = _mapper.Map<Account>(request);

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account created successfully. AccountId: {AccountId}", account.Id);

            return _mapper.Map<AccountDetailResponse>(account);
        }
        /// <inheritdoc/>
        public async Task UpdateAsync(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Updating account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);
                throw NotFoundException.For<Account>(id);
            }

            var takenName = await _unitOfWork.Accounts.IsNameTakenAsync(request.Name, excludeId: id);

            if (takenName)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);
                throw new BusinessException($"Account with '{request.Name}' already exists.");
            }

            account.Name = request.Name;
            account.Description = request.Description;
            account.Type = request.Type;
            account.IsActive = request.IsActive;

            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account updated successfully. AccountId: {AccountId}", id);
        }
        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Soft deleting account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);
                throw NotFoundException.For<Account>(id);
            }

            _unitOfWork.Accounts.Delete(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account soft deleted successfully. AccountId: {AccountId}", id);
        }
        /// <inheritdoc/>
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdIncludingDeletedAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);
                throw NotFoundException.For<Account>(id);
            }

            var takenName = await _unitOfWork.Accounts.IsNameTakenAsync(account.Name, excludeId: id);

            if (takenName)
            {
                _logger.LogWarning("Account's name already exists. AccountName: {AccountName}", account.Name);
                throw new BusinessException($"Account with '{account.Name}' already exists.");
            }

            account.IsDeleted = false;

            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account restored successfully. AccountId: {AccountId}", id);
        }
    }
}
