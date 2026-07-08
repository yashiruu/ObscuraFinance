using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Application.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null);
    }
}
