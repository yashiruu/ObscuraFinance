using Obscura.FinanceTracker.Base.Entities;
using System.Linq.Expressions;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        // Read
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        // Write
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        // Add on
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predication);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predication);

        // Persistance
        Task<int> SaveChangeAsync();
    }
}
