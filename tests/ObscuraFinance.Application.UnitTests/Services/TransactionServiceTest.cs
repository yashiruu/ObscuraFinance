using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Responses;
using Obscura.FinanceTracker.Application.Interfaces.Repositories;
using Obscura.FinanceTracker.Application.Services;
using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;
using ObscuraFinance.Application.UnitTests.Base;
using System.Linq.Expressions;

namespace ObscuraFinance.Application.UnitTests.Services
{
    /// <summary>
    /// Comprehensive unit tests for <see cref="TransactionService"/> covering all business scenarios.
    /// </summary>
    public class TransactionServiceTest : TransactionServiceTestBase
    {
        #region GetAllAsync Tests

        /// <summary>
        /// Verifies that GetAllAsync retrieves transactions from the repository with details and returns mapped list.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_Should_ReturnMappedList_When_TransactionsExist()
        {
            // Arrange
            var transactions = new List<Transaction> { new Transaction { Id = Guid.NewGuid(), Name = "Lunch" } };
            var expectedResponse = new List<TransactionListResponse> { new TransactionListResponse { Name = "Lunch" } };

            // Assuming TransactionService uses GetAllWithDetailAsync or GetAllAsync. We mock both just in case, 
            // but typical pattern uses GetWithDetails for Lists.
            _transactionRepositoryMock.Setup(repo => repo.GetAllWithDetailAsync()).ReturnsAsync(transactions);
            _transactionRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(transactions);

            _mapperMock.Setup(m => m.Map<IEnumerable<TransactionListResponse>>(transactions)).Returns(expectedResponse);

            // Act
            var result = await _transactionService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        #endregion

        #region GetByIdAsync Tests

        /// <summary>
        /// Verifies that GetByIdAsync returns a mapped transaction when the ID is valid.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ReturnMappedTransaction_When_TransactionExists()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var transaction = new Transaction { Id = transactionId, Name = "Lunch" };
            var expectedResponse = new TransactionDetailResponse { Id = transactionId, Name = "Lunch" };

            // Mock both GetByIdAsync and GetByIdWithDetailAsync to cover either implementation
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync(transaction);
            _transactionRepositoryMock.Setup(repo => repo.GetByIdWithDetailAsync(transactionId)).ReturnsAsync(transaction);

            _mapperMock.Setup(m => m.Map<TransactionDetailResponse>(transaction)).Returns(expectedResponse);

            // Act
            var result = await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        /// <summary>
        /// Verifies that GetByIdAsync throws a KeyNotFoundException when the transaction is missing.
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_Should_ThrowKeyNotFoundException_When_TransactionDoesNotExist()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);
            _transactionRepositoryMock.Setup(repo => repo.GetByIdWithDetailAsync(transactionId)).ReturnsAsync((Transaction?)null);

            // Act
            var act = async () => await _transactionService.GetByIdAsync(transactionId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*Transaction with '{transactionId}' was not found*");
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Verifies that CreateAsync successfully persists a new transaction when the request is valid.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_CreateAndReturnResponse_When_RequestIsValid()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var request = new TransactionCreateRequest { Name = "Salary", Type = TransactionType.Income, Amount = 5000 };
            var transactionEntity = new Transaction { Id = Guid.NewGuid(), Name = request.Name, Type = request.Type, Amount = request.Amount };
            var expectedResponse = new TransactionDetailResponse { Id = transactionEntity.Id, Name = request.Name, Type = request.Type };

            _mapperMock.Setup(m => m.Map<Transaction>(request)).Returns(transactionEntity);
            _mapperMock.Setup(m => m.Map<TransactionDetailResponse>(transactionEntity)).Returns(expectedResponse);

            // Act
            var result = await _transactionService.CreateAsync(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResponse);
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(transactionEntity), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that CreateAsync throws a KeyNotFoundException when the specified account does not exist.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_ThrowKeyNotFoundException_When_AccountDoesNotExist()
        {
            // Arrange
            var request = new TransactionCreateRequest
            {
                Name = "Salary",
                Type = TransactionType.Income,
                Amount = 5000,
                AccountId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            };

            _accountRepositoryMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var act = () => _transactionService.CreateAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Account with `{request.AccountId}` was not found");

            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// Verifies that CreateAsync throws a KeyNotFoundException when the specified category does not exist.
        /// </summary>
        [Fact]
        public async Task CreateAsync_Should_ThrowKeyNotFoundException_When_CategoryDoesNotExist()
        {
            // Arrange
            var request = new TransactionCreateRequest
            {
                Name = "Salary",
                Type = TransactionType.Income,
                Amount = 5000,
                AccountId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            };

            _categoryRepositoryMock
                .Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var act = () => _transactionService.CreateAsync(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Category with `{request.CategoryId}` was not found");

            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Verifies that UpdateAsync updates existing transaction successfully when data is valid.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_UpdateTransaction_When_RequestIsValid()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var request = new TransactionUpdateRequest { Name = "Updated Lunch", Amount = 150 };
            var existingTransaction = new Transaction { Id = transactionId, Name = "Old Lunch" };

            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync(existingTransaction);

            // Act
            var act = async () => await _transactionService.UpdateAsync(transactionId, request, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            _transactionRepositoryMock.Verify(repo => repo.Update(existingTransaction), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that UpdateAsync throws a KeyNotFoundException when the target transaction does not exist.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_Should_ThrowKeyNotFoundException_When_TransactionDoesNotExist()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var request = new TransactionUpdateRequest { Name = "Updated Lunch" };

            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

            // Act
            var act = async () => await _transactionService.UpdateAsync(transactionId, request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*Transaction with '{transactionId}' was not found*");
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Verifies that DeleteAsync delegates the deletion process to the repository successfully.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_DeleteTransaction_When_TransactionExists()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var transaction = new Transaction { Id = transactionId };

            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync(transaction);

            // Act
            var act = async () => await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            _transactionRepositoryMock.Verify(repo => repo.Delete(transaction), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that DeleteAsync throws a KeyNotFoundException when attempting to delete a non-existent transaction.
        /// </summary>
        [Fact]
        public async Task DeleteAsync_Should_ThrowKeyNotFoundException_When_TransactionDoesNotExist()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            _transactionRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

            // Act
            var act = async () => await _transactionService.DeleteAsync(transactionId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*Transaction with '{transactionId}' was not found*");

            _transactionRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region RestoreAsync Tests

        /// <summary>
        /// Verifies that RestoreAsync successfully marks a soft-deleted transaction as active.
        /// </summary>
        [Fact]
        public async Task RestoreAsync_Should_RestoreTransaction_When_TransactionExists()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var transaction = new Transaction { Id = transactionId, Name = "Netflix Sub", IsDeleted = true };

            _transactionRepositoryMock.Setup(repo => repo.GetByIdIncludingDeletedAsync(transactionId)).ReturnsAsync(transaction);

            // Act
            var act = async () => await _transactionService.RestoreAsync(transactionId, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            transaction.IsDeleted.Should().BeFalse(); // Validates the domain logic modification
            _transactionRepositoryMock.Verify(repo => repo.Update(transaction), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that RestoreAsync throws KeyNotFoundException if restoring a missing transaction.
        /// </summary>
        [Fact]
        public async Task RestoreAsync_Should_ThrowKeyNotFoundException_When_TransactionDoesNotExist()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            _transactionRepositoryMock.Setup(repo => repo.GetByIdIncludingDeletedAsync(transactionId)).ReturnsAsync((Transaction?)null);

            // Act
            var act = async () => await _transactionService.RestoreAsync(transactionId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*Transaction with '{transactionId}' was not found*");

            _transactionRepositoryMock.Verify(repo => repo.Update(It.IsAny<Transaction>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion
    }
}