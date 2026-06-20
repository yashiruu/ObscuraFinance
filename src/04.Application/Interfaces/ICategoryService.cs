using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;

namespace Obscura.FinanceTracker.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<CategoryResponse>> GetByTypeAsync(int type, CancellationToken cancellationToken);
        Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken);
        Task<CategoryResponse?> GetByIdASync(Guid id, CancellationToken cancellationToken);
        Task CreateAsync(CategoryCreateRequest request ,CancellationToken cancellationToken);
        Task UpdateAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task RestoreASync(Guid id, CancellationToken cancellationToken);
    }
}
