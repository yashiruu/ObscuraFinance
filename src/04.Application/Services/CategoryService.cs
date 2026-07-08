using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.DTOs.Categories.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Interfaces.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Exceptions;

namespace Obscura.FinanceTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryCreateRequest> _createValidator;
        private readonly IValidator<CategoryUpdateRequest> _updateValidator;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IValidator<CategoryCreateRequest> createValidator, IValidator<CategoryUpdateRequest> updateValidator, ILogger<CategoryService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all categories");

            var categories = await _unitOfWork.Categories.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} categories", categories.Count);

            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<IEnumerable<CategoryResponse>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving categories by type. Type: {CategoryType}", type);

            var categories = await _unitOfWork.Categories.GetAllByTypeAsync(type);

            _logger.LogInformation("Retrieved {Count} categories for type {CategoryType}", categories.Count, type);

            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<IEnumerable<CategoryResponse>> GetDeletedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving deleted categories");

            var categories = await _unitOfWork.Categories.GetAllDeletedAsync();

            _logger.LogInformation("Retrieved {Count} deleted categories", categories.Count);

            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            _logger.LogInformation("Category retrieved successfully. CategoryId: {CategoryId}", id);

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> CreateAsync(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Creating category. CategoryName: {CategoryName}", request.Name);

            var takenName = await _unitOfWork.Categories.IsNameTakenAsync(request.Name);

            if (takenName)
            {
                _logger.LogWarning("Category's name already exists. CategoryName: {CategoryName}", request.Name);
                throw new BusinessException($"Category with '{request.Name}' already exists.");
            }

            var category = _mapper.Map<Category>(request);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category created successfully. CategoryId: {CategoryId}", category.Id);

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task UpdateAsync(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);

            _logger.LogInformation("Updating category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null) 
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with '{id}' was not found.");
            }

            var takenName = await _unitOfWork.Categories.IsNameTakenAsync(request.Name, excludeId: id);

            if (takenName)
            {
                _logger.LogWarning("Category's name already exists. CategoryName: {CategoryName}", request.Name);
                throw new BusinessException($"Category with '{request.Name}' already exists.");
            }

            category.Name = request.Name;
            category.Description = request.Description;
            category.Type = request.Type;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category updated successfully. CategoryId: {CategoryId}", id);
        }   

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Soft deleting category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category deleted successfully. CategoryId: {CategoryId}", id);
        }

        public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Restoring category. CategoryId: {CategoryId}", id);

            var category = await _unitOfWork.Categories.GetByIdIncludingDeletedAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found. CategoryId: {CategoryId}", id);
                throw new KeyNotFoundException($"Category with '{id}' was not found");
            }

            var takenName = await _unitOfWork.Categories.IsNameTakenAsync(category.Name, excludeId: id);

            if (takenName)
            {
                _logger.LogWarning("Category's name already exists. CategoryName: {CategoryName}", category.Name);
                throw new BusinessException($"Category with '{category.Name}' already exists.");
            }

            category.IsDeleted = false;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category restored successfully. CategoryId: {CategoryId}", id);
        }
    }
}
