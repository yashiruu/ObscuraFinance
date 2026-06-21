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
        private readonly AppDbContext _context;
        private readonly ILogger<AccountService> _logger;

        public AccountService(AppDbContext context, ILogger<AccountService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<AccountListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all accounts");

            var accounts = await _context.Accounts
                .OrderBy(a => a.Name)
                .Select(a => new AccountListResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    CurrentBalance = a.CurrentBalance,
                    Type = a.Type,
                    IsActive = a.IsActive
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} accounts", accounts.Count);

            return accounts;
        }
        public async Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving account. AccountId: {AccountId}", id);

            var account = await _context.Accounts
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
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            _logger.LogInformation("Account retrieved successfully. AccountId: {AccountId}", id);

            return account;
        }
        public async Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating account. AccountName: {AccountName}", request.Name);

            var exists = await _context.Accounts
                .IgnoreQueryFilters()
                .AnyAsync(a => a.Name == request.Name, cancellationToken);

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

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

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

            var exists = await _context.Accounts
                .IgnoreQueryFilters()
                .AnyAsync(a => a.Id != id && a.Name == request.Name, cancellationToken);

            if (exists)
            {
                _logger.LogWarning("Account already exists. AccountName: {AccountName}", request.Name);

                throw new ValidationException($"Account with '{request.Name}' already exists.");
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            account.Name = request.Name;
            account.Description = request.Description;
            account.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account updated successfully. AccountId: {AccountId}", id);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Soft deleting account. AccountId: {AccountId}", id);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found");
            }

            account.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account soft deleted successfully. AccountId: {AccountId}", id);
        }
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring account. AccountId: {AccountId}", id);
            var account = await _context.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted, cancellationToken);

            if (account == null)
            {
                _logger.LogWarning("Account not found. AccountId: {AccountId}", id);

                throw new KeyNotFoundException($"Account with '{id}' was not found");
            }

            account.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account restored successfully. AccountId: {AccountId}", id);
        }
    }
}
