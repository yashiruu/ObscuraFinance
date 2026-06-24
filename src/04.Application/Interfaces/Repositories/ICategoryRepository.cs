using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetAllByTypeAsync(TransactionType type);
        Task<IReadOnlyList<Category>> GetAllDeletedAsync();
    }
}
