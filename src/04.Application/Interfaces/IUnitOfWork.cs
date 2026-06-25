using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // --- Repositories ---
        // Each repository is exposed here so the service only needs
        // one dependency: IUnitOfWork. No more injecting three separate
        // repositories into every service constructor.

        IRepository<Account> Accounts { get; }
        ICategoryRepository Categories { get; }
        IRepository<Transaction> Transactions { get; }

        // --- Commit ---
        // All tracked changes are persisted to the database

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
