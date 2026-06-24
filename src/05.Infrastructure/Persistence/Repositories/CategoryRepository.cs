using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
        public async Task<IReadOnlyList<Category>> GetAllByTypeAsync(TransactionType type)
        {
            return await _dbSet
                .Where(c => c.Type == type)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetAllDeletedAsync()
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .ToListAsync();
        }
    }
}
