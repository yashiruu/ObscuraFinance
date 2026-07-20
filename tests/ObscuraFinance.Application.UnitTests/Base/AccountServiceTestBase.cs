using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;
using ObscuraFinance.Application.UnitTests.Mocks;

namespace ObscuraFinance.Application.UnitTests.Base
{
    public abstract class AccountServiceTestBase : ServiceTestBase
    {
        protected readonly Mock<IAccountRepository> _accountRepositoryMock;
        protected readonly Mock<IValidator<AccountCreateRequest>> _accountCreateValidatorMock;
        protected readonly Mock<IValidator<AccountUpdateRequest>> _accountUpdateValidatorMock;
        protected readonly Mock<ILogger<AccountService>> _loggerMock;

        protected readonly AccountService _accountService;

        protected AccountServiceTestBase()
        {
            _accountRepositoryMock = new();
            _unitOfWorkMock.Setup(uow => uow.Accounts).Returns(_accountRepositoryMock.Object);
            _accountCreateValidatorMock = new();
            _accountUpdateValidatorMock = new();
            _loggerMock = LoggerMockFactory.Create<AccountService>();

            _accountService = new AccountService(
                unitOfWork: _unitOfWorkMock.Object,
                createValidator: _accountCreateValidatorMock.Object,
                updateValidator: _accountUpdateValidatorMock.Object,
                logger: _loggerMock.Object,
                mapper: _mapperMock.Object
            );
        }
    }
}
