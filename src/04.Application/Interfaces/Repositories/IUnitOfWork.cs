namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // --- Repositories ---
        // Each repository is exposed here so the service only needs
        // one dependency: IUnitOfWork. No more injecting three separate
        // repositories into every service constructor.
        IAccountRepository Accounts { get; }
        ICategoryRepository Categories { get; }
        ITransactionRepository Transactions { get; }

        // --- Commit ---
        // All tracked changes are persisted to the database
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
