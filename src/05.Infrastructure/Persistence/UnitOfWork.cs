using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Infrastructure.Persistence.Repositories;

namespace Obscura.FinanceTracker.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IRepository<Account>? _accounts;
        private ICategoryRepository? _categories;
        private IRepository<Transaction>? _transactions;
        
        public UnitOfWork(AppDbContext context) 
        {
            _context = context;
        }

        // --- Repository Access ---
        // Each property initializes the repository on first access,
        // passing the shared _context instance into it.
        // This ensures all repositories share the same change tracker.
        public IRepository<Account> Accounts => _accounts ??= new Repository<Account>(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public IRepository<Transaction> Transactions => _transactions ??= new Repository<Transaction>(_context);

        // --- Commit ---
        // One call persists all tracked changes from all repositories.
        // This is the entire point of Unit of Work.
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        // --- Cleanup ---
        // Disposing UnitOfWork disposes the DbContext.
        public void Dispose() => _context.Dispose();
    }
}
