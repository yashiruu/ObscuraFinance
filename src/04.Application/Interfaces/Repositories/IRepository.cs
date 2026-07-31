using Obscura.FinanceTracker.Base.Entities;
using System.Linq.Expressions;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Read
        /// <summary>
        /// Retrieves a paged list of entities along with the total count.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A tuple containing the collection of items and the total record count.</returns>
        Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetAllAsync(int PageNumber, int PageSize, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity?> GetByIdIncludingDeletedAsync(Guid id);
        #endregion

        Task<IReadOnlyList<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        // Write
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        // Add on
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predication);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predication);
    }
}
