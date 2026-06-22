using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Domain.Enums;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken);
        Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken);
        Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<CategoryResponse> CreateAsync(CategoryCreateRequest request ,CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task RestoreAsync(Guid id, CancellationToken cancellationToken);
    }
}
