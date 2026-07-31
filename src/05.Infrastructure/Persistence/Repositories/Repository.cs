using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Base.Entities;
using System;
using System.Linq.Expressions;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region Read
        public async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await _dbSet.CountAsync(cancellationToken);

            // Apply Skip/Take pattern for efficient querying. A deterministic order is required:
            // without it the database may return rows in a different order per query, causing
            // duplicated or skipped rows across pages.
            var items = await _dbSet
                .AsNoTracking() // Optimization for read-only queries
                .OrderBy(x => x.CreatedAt)
                .ThenBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> GetByIdIncludingDeletedAsync(Guid id)
        {
            return await _dbSet.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region write
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Add on
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.IgnoreQueryFilters().AnyAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
        #endregion
    }
}
