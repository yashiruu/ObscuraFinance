using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IReadOnlyList<Transaction>> GetAllWithDetailAsync();
    }
}
