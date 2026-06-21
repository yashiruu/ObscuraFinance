using Microsoft.EntityFrameworkCore;
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

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountListResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
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

            return accounts;
        }
        public async Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Where(a => a.Id == id)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            return account;
        }
        public async Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken)
        {
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

            var exists = await _context.Accounts
                .IgnoreQueryFilters()
                .AnyAsync(a => a.Name == request.Name, cancellationToken);

            if (exists)
            {
                throw new ValidationException($"Account with '{account.Name}' already exists.");
            }

            await _context.Accounts.AddAsync(account, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

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
            var exists = await _context.Accounts
                .IgnoreQueryFilters()
                .AnyAsync(a =>a.Name == request.Name, cancellationToken);

            if (exists)
            {
                throw new ValidationException($"Account with '{request.Name}' already exists.");
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with '{id}' was not found.");
            }

            account.Name = request.Name;
            account.Description = request.Description;
            account.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with '{id}' was not found or has been deleted");
            }

            account.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted, cancellationToken);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with '{id}' was not found or has been restored");
            }

            account.IsDeleted = false;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
