using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;
using Obscura.FinanceTracker.Domain.Entities;
using ObscuraFinance.Application.UnitTests.Mocks;
using System.Linq.Expressions;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class TransactionServiceTestBase : ServiceTestBase
    {
        protected readonly Mock<IAccountRepository> _accountRepositoryMock;
        protected readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        protected readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        protected readonly Mock<IValidator<TransactionCreateRequest>> _transactionCreateValidatorMock;
        protected readonly Mock<IValidator<TransactionUpdateRequest>> _transactionUpdateValidatorMock;
        protected readonly Mock<ILogger<TransactionService>> _loggerMock;

        protected readonly TransactionService _transactionService;

        protected TransactionServiceTestBase()
        {
            _accountRepositoryMock = new();
            _categoryRepositoryMock = new();
            _transactionRepositoryMock = new();

            _unitOfWorkMock
                .Setup(uow => uow.Accounts)
                .Returns(_accountRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(uow => uow.Categories)
                .Returns(_categoryRepositoryMock.Object);
            
            _unitOfWorkMock.Setup(uow => uow.Transactions).Returns(_transactionRepositoryMock.Object);

            // Default behavior
            _accountRepositoryMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(true);

            _categoryRepositoryMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            _transactionCreateValidatorMock = new();
            _transactionUpdateValidatorMock = new();
            _loggerMock = LoggerMockFactory.Create<TransactionService>();

            _transactionService = new TransactionService(
                unitOfWork: _unitOfWorkMock.Object,
                createValidator: _transactionCreateValidatorMock.Object,
                updateValidator: _transactionUpdateValidatorMock.Object,
                logger: _loggerMock.Object,
                mapper: _mapperMock.Object
            );
        }
    }
}
