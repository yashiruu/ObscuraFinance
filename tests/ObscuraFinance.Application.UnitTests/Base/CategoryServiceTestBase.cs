using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;
using ObscuraFinance.Application.UnitTests.Mocks;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class CategoryServiceTestBase : ServiceTestBase
    {
        protected readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        protected readonly Mock<IValidator<CategoryCreateRequest>> _categoryCreateValidatorMock;
        protected readonly Mock<IValidator<CategoryUpdateRequest>> _categoryUpdateValidatorMock;
        protected readonly Mock<ILogger<CategoryService>> _loggerMock;
        
        protected readonly CategoryService _categoryService;

        protected CategoryServiceTestBase()
        {
            _categoryRepositoryMock = new();
            _unitOfWorkMock.Setup(uow => uow.Categories).Returns(_categoryRepositoryMock.Object);
            _categoryCreateValidatorMock = new();
            _categoryUpdateValidatorMock = new();
            _loggerMock = LoggerMockFactory.Create<CategoryService>();

            _categoryService = new CategoryService(
                unitOfWork: _unitOfWorkMock.Object,
                createValidator: _categoryCreateValidatorMock.Object,
                updateValidator: _categoryUpdateValidatorMock.Object,
                logger: _loggerMock.Object,
                mapper: _mapperMock.Object
            );
        }
    }
}
