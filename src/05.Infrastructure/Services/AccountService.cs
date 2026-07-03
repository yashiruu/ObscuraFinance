using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Shared.Exceptions;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AccountCreateRequest> _validator;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IValidator<AccountCreateRequest> validator, ILogger<AccountService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all accounts");

            var accounts = await _unitOfWork.Accounts.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} accounts", accounts.Count);

            return _mapper.Map<IEnumerable<AccountListResponse>>(accounts);
        }
        public async Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            _logger.LogInformation("Account retrieved successfully. AccountId: {AccountId}", id);

            return _mapper.Map<AccountDetailResponse>(account);
        }
        public async Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Creating account. AccountName: {AccountName}", request.Name);

            var exists = await _unitOfWork.Accounts.ExistsAsync(a => a.Name == request.Name);

            if (exists)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);

                throw new BusinessException($"Account with '{request.Name}' already exists.");
            }

            var account = _mapper.Map<Account>(request);

            await _unitOfWork.Accounts.AddAsync(account);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account created successfully. AccountId: {AccountId}", account.Id);

            return _mapper.Map<AccountDetailResponse>(account);
        }
        public async Task UpdateAsync(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating account. AccountId: {AccountId}", id);

            var exists = await _unitOfWork.Accounts.ExistsAsync(a => a.Id != id && a.Name == request.Name);

            if (exists)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);

                throw new BusinessException($"Account with '{request.Name}' already exists.");
            }

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            account.Name = request.Name;
            account.Description = request.Description;
            account.Type = request.Type;
            account.IsActive = request.IsActive;

            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account updated successfully. AccountId: {AccountId}", id);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Soft deleting account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found");
            }

            _unitOfWork.Accounts.Delete(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account soft deleted successfully. AccountId: {AccountId}", id);
        }
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring account. AccountId: {AccountId}", id);

            var account = await _unitOfWork.Accounts.GetByIdIncludingDeletedAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found");
            }

            account.IsDeleted = false;

            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account restored successfully. AccountId: {AccountId}", id);
        }
    }
}
