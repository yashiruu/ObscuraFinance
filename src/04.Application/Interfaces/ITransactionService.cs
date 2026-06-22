using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionListResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<TransactionDetailResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<TransactionDetailResponse> CreateAsync(TransactionCreateRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, TransactionUpdateRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
