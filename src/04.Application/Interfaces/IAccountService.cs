using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountListResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<AccountDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<AccountDetailResponse> CreateAsync(AccountCreateRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, AccountUpdateRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
