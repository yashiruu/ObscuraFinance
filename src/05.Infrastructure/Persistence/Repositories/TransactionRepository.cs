using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Transaction?> GetByIdWithDetailAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IReadOnlyList<Transaction>> GetAllWithDetailAsync()
        {
            return await _dbSet
                .OrderByDescending(t => t.Date)
                .Include(t => t.Account)
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
}
