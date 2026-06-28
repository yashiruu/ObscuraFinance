using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }

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
