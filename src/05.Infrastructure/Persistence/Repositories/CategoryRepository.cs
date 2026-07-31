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

        public async Task<(IReadOnlyList<Category> Items, int TotalCount)> GetAllDeletedAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.IgnoreQueryFilters().Where(c => c.IsDeleted);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.DeletedAt)
                .ThenBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null)
        {
            return await _dbSet.AnyAsync(a => a.Name == name && (excludeId == null || a.Id != excludeId));
        }
    }
}
