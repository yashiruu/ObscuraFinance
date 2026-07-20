using FluentAssertions;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Exceptions;
using ObscuraFinance.Application.UnitTests.Base;
using ObscuraFinance.Application.UnitTests.Builders;

namespace ObscuraFinance.Application.UnitTests.Services
{
    public class AccountServiceTest : AccountServiceTestBase
    {
        // ============================================================
        // CreateAsync
        // ============================================================

        /// <summary>
        /// Verifies that <see cref="AccountService.CreateAsync"/> successfully creates
        /// an account and returns its detail response when the request is valid
        /// (i.e. the account name is not already taken).
        ///
        /// This test focuses on orchestration, not data mapping correctness:
        /// - Confirms the service checks name uniqueness before proceeding.
        /// - Confirms the mapped entity is persisted via the repository and Unit Of Work.
        /// - Confirms the returned response reflects the newly created account.
        ///
        /// FluentValidation rules are not exercised here (the validator is mocked to
        /// always pass); validation behavior is covered separately in the dedicated
        /// validator test suite.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_CreateAccount_When_ValidRequest()
        {
            // Arrange
            var request = new AccountCreateRequest
            {
                Name = "Savings Account",
                Description = "My savings",
                InitialBalance = 100_000m,
                Currency = "IDR",
                Type = AccountType.Bank
            };

            var mappedAccount = AccountBuilder.Create()
                .WithName(request.Name)
                .Build();

            _accountRepositoryMock
                .Setup(repo => repo.IsNameTakenAsync(request.Name, null))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Account>(request))
                .Returns(mappedAccount);

            _mapperMock
                .Setup(m => m.Map<AccountDetailResponse>(mappedAccount))
                .Returns(new AccountDetailResponse
                {
                    Id = mappedAccount.Id,
                    Name = mappedAccount.Name
                });

            // Act
            var result = await _accountService.CreateAsync(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);

            // Orchestration verification: ensure the repository and unit of work were called as expected
            _accountRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Account>(a => a.Name == request.Name)),
                Times.Once);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that <see cref="AccountService.CreateAsync"/> throws
        /// <see cref="BusinessException"/> when the requested account name is
        /// already taken, and that no persistence side effects occur.
        ///
        /// This test intentionally does NOT set up the mapper, because the
        /// service is expected to throw before reaching the mapping step
        /// (name-uniqueness check happens first). If a future change to
        /// AccountService reorders these steps, this test will fail loudly
        /// due to the missing mapper setup — which is a desirable signal.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_ThrowBusinessException_When_NameAlreadyExists()
        {
            // Arrange
            var request = new AccountCreateRequest
            {
                Name = "Existing Account",
                Description = "Duplicate name test",
                InitialBalance = 50_000m,
                Currency = "IDR",
                Type = AccountType.Bank
            };

            _accountRepositoryMock
                .Setup(repo => repo.IsNameTakenAsync(request.Name, null))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _accountService.CreateAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"*{request.Name}*");  // Using wildcard to match the exception message containing the account name (not strict)

            // Orchestration verification: ensure the repository and unit of work were not called since the name check failed
            _accountRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Account>(a => a.Name == request.Name)),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        // ============================================================
        // GetByIdAsync
        // ============================================================

        /// <summary>
        /// Verifies that <see cref="AccountService.GetByIdAsync"/> returns a mapped
        /// <see cref="AccountDetailResponse"/> when the account exists.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ReturnMappedResponse_When_AccountExists()
        {
            // Arrange
            var account = AccountBuilder.Create().Build();

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(account.Id))
                .ReturnsAsync(account);

            _mapperMock
                .Setup(m => m.Map<AccountDetailResponse>(account))
                .Returns(new AccountDetailResponse
                {
                    Id = account.Id,
                    Name = account.Name
                });

            // Act
            var result = await _accountService.GetByIdAsync(account.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(account.Id);
        }

        /// <summary>
        /// Verifies that <see cref="AccountService.GetByIdAsync"/> throws
        /// <see cref="KeyNotFoundException"/> when no account matches the given id.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ThrowKeyNotFoundException_When_AccountDoesNotExist()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account?)null);

            // Act
            var act = async () => await _accountService.GetByIdAsync(accountId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{accountId}*");
        }

        // ============================================================
        // GetAllAsync
        // ============================================================

        /// <summary>
        /// Verifies that <see cref="AccountService.GetAllAsync"/> returns an empty
        /// collection (not null, not an exception) when no accounts exist.
        ///
        /// This guards against a common service-layer mistake: assuming at least
        /// one record always exists and failing on empty collections.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_Should_ReturnEmptyList_When_NoAccountsExist()
        {
            // Arrange
            _accountRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Account>());

            _mapperMock
                .Setup(m => m.Map<IEnumerable<AccountListResponse>>(It.IsAny<IEnumerable<Account>>()))
                .Returns(new List<AccountListResponse>());

            // Act
            var result = await _accountService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        // ============================================================
        // UpdateAsync
        // ============================================================

        /// <summary>
        /// Verifies that <see cref="AccountService.UpdateAsync"/> updates an existing
        /// account's fields and persists the change when the request is valid.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_UpdateAccount_When_ValidRequest()
        {
            // Arrange
            var existingAccount = AccountBuilder.Create()
                .WithName("Old Name")
                .Build();

            var request = new AccountUpdateRequest
            {
                Name = "New Name",
                Description = "Updated description",
                Type = AccountType.Bank,
                IsActive = true
            };

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(existingAccount.Id))
                .ReturnsAsync(existingAccount);

            _accountRepositoryMock
                .Setup(repo => repo.IsNameTakenAsync(request.Name, existingAccount.Id))
                .ReturnsAsync(false);

            // Act
            await _accountService.UpdateAsync(existingAccount.Id, request, CancellationToken.None);

            // Assert
            existingAccount.Name.Should().Be(request.Name);
            existingAccount.Description.Should().Be(request.Description);

            _accountRepositoryMock.Verify(
                repo => repo.Update(existingAccount),
                Times.Once);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that <see cref="AccountService.UpdateAsync"/> throws
        /// <see cref="KeyNotFoundException"/> when the target account does not exist,
        /// and that no name-uniqueness check or persistence occurs afterward.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_ThrowKeyNotFoundException_When_AccountDoesNotExist()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var request = new AccountUpdateRequest
            {
                Name = "Doesn't Matter",
                Type = AccountType.Bank,
                IsActive = true
            };

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account?)null);

            // Act
            var act = async () => await _accountService.UpdateAsync(accountId, request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{accountId}*");

            _accountRepositoryMock.Verify(
                repo => repo.IsNameTakenAsync(It.IsAny<string>(), It.IsAny<Guid?>()),
                Times.Never);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        /// <summary>
        /// Verifies that <see cref="AccountService.UpdateAsync"/> throws
        /// <see cref="BusinessException"/> when the new name is already taken by
        /// another account.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_ThrowBusinessException_When_NewNameAlreadyTaken()
        {
            // Arrange
            var existingAccount = AccountBuilder.Create()
                .WithName("Old Name")
                .Build();

            var request = new AccountUpdateRequest
            {
                Name = "Taken Name",
                Type = AccountType.Bank,
                IsActive = true
            };

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(existingAccount.Id))
                .ReturnsAsync(existingAccount);

            _accountRepositoryMock
                .Setup(repo => repo.IsNameTakenAsync(request.Name, existingAccount.Id))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _accountService.UpdateAsync(existingAccount.Id, request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"*{request.Name}*");

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        /// <summary>
        /// GAP-EXPOSING TEST — documents AO-004 Gap 2 (known-issues.md).
        ///
        /// Current behavior: AccountService.UpdateAsync always calls
        /// IsNameTakenAsync, even when the requested name is identical to the
        /// account's existing name. This test asserts today's actual behavior
        /// (the redundant call still happens), NOT the ideal behavior.
        ///
        /// This test is expected to start FAILING once AO-004 Gap 2 is fixed
        /// (i.e. once the service short-circuits the check for unchanged names).
        /// When that happens, flip this test to assert Times.Never instead of
        /// Times.Once, and remove this AO-004 note.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_StillCallNameTakenCheck_When_NameIsUnchanged_AO004()
        {
            // Arrange
            var existingAccount = AccountBuilder.Create()
                .WithName("Unchanged Name")
                .Build();

            var request = new AccountUpdateRequest
            {
                Name = "Unchanged Name", // same as existing
                Type = AccountType.Bank,
                IsActive = true
            };

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(existingAccount.Id))
                .ReturnsAsync(existingAccount);

            _accountRepositoryMock
                .Setup(repo => repo.IsNameTakenAsync(request.Name, existingAccount.Id))
                .ReturnsAsync(false);

            // Act
            await _accountService.UpdateAsync(existingAccount.Id, request, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(
                repo => repo.IsNameTakenAsync(request.Name, existingAccount.Id),
                Times.Once); // documents current (redundant) behavior — see AO-004 Gap 2
        }

        // ============================================================
        // DeleteAsync
        // ============================================================

        /// <summary>
        /// Verifies that <see cref="AccountService.DeleteAsync"/> soft-deletes an
        /// existing account and persists the change.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_SoftDeleteAccount_When_AccountExists()
        {
            // Arrange
            var account = AccountBuilder.Create().Build();

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(account.Id))
                .ReturnsAsync(account);

            // Act
            await _accountService.DeleteAsync(account.Id, CancellationToken.None);

            // Assert
            _accountRepositoryMock.Verify(
                repo => repo.Delete(account),
                Times.Once);

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that <see cref="AccountService.DeleteAsync"/> throws
        /// <see cref="KeyNotFoundException"/> when the target account does not exist.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_ThrowKeyNotFoundException_When_AccountDoesNotExist()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account?)null);

            // Act
            var act = async () => await _accountService.DeleteAsync(accountId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{accountId}*");

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        /// <summary>
        /// GAP-EXPOSING TEST — documents AO-004 Gap 3 (known-issues.md), Medium risk.
        ///
        /// Current behavior: AccountService.DeleteAsync soft-deletes an account
        /// without checking whether it is still referenced by active transactions.
        /// This test asserts today's actual behavior (delete still succeeds),
        /// NOT the ideal behavior.
        ///
        /// This test is expected to start FAILING once AO-004 Gap 3 is fixed for
        /// Account (i.e. once a referential-integrity check is added). When that
        /// happens, update this test to assert the appropriate exception instead.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_StillSucceed_When_AccountHasReferencedTransactions_AO004()
        {
            // Arrange
            var account = AccountBuilder.Create().Build();

            _accountRepositoryMock
                .Setup(repo => repo.GetByIdAsync(account.Id))
                .ReturnsAsync(account);

            // NOTE: no transaction-reference check exists yet in AccountService,
            // so no additional setup is needed to simulate "referenced by transactions".

            // Act
            var act = async () => await _accountService.DeleteAsync(account.Id, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync(); // documents current gap — see AO-004 Gap 3

            _unitOfWorkMock.Verify(
                uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
