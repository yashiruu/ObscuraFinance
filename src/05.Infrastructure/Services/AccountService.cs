using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Obscura.FinanceTracker.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<AccountListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all accounts");

            var accounts = await _unitOfWork.Accounts.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} accounts", accounts.Count);

            return accounts.Select(account => new AccountListResponse
            {
                Id = account.Id,
                Name= account.Name,
                Type = account.Type,
                CurrentBalance = account.CurrentBalance,
                IsActive = account.IsActive
            });
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

            return new AccountDetailResponse
            {
                Id = account.Id,
                Name = account.Name,
                Description = account.Description,
                CurrentBalance = account.CurrentBalance,
                InitialBalance = account.InitialBalance,
                Currency = account.Currency,
                Type = account.Type,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt
            };
        }
        public async Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating account. AccountName: {AccountName}", request.Name);

            var exists = await _unitOfWork.Accounts.ExistsAsync(a => a.Name == request.Name);

            if (exists)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);

                throw new ValidationException($"Account with '{request.Name}' already exists.");
            }

            var account = new Account
            {
                Name = request.Name,
                Description = request.Description,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                Currency = request.Currency,
                Type = request.Type,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(account);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account created successfully. AccountId: {AccountId}", account.Id);

            return new AccountDetailResponse
            {
                Id = account.Id,
                Name = request.Name,
                Description = request.Description,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                Currency = request.Currency,
                Type = request.Type,
                IsActive = true
            };
        }
        public async Task UpdateAsync(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating account. AccountId: {AccountId}", id);

            var exists = await _unitOfWork.Accounts.ExistsAsync(a => a.Id != id && a.Name == request.Name);

            if (exists)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);

                throw new ValidationException($"Account with '{request.Name}' already exists.");
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
