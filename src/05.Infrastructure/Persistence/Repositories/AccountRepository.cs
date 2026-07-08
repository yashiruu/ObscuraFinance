using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;

namespace Obscura.FinanceTracker.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null)
        {
            return await _dbSet.AnyAsync(a => a.Name == name && (excludeId == null || a.Id != excludeId));
        }
    }
}
