using FluentValidation.TestHelper;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Application.Validators.Transaction;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Constants;
using System;
using Xunit;

namespace ObscuraFinance.Application.UnitTest.Validators.Transaction
{
    public class TransactionUpdateRequestValidatorTest
    {
        private readonly TransactionUpdateRequestValidator _validator;

        public TransactionUpdateRequestValidatorTest()
        {
            _validator = new TransactionUpdateRequestValidator();
        }

        private static TransactionUpdateRequest CreateValidRequest()
        {
            return new TransactionUpdateRequest
            {
                Date = DateTime.Today,
                Name = "Grocery Shopping",
                Amount = 100,
                Type = TransactionType.Expense,
                AccountId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            };
        }

        // ---------- Date ----------

        [Fact]
        public void Date_Should_HaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Date = default;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("Transaction date is required.");
        }

        [Fact]
        public void Date_Should_HaveError_When_EarlierThanMinimumDate()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Date = TransactionConstraints.MinimumDate.AddDays(-1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("Transaction date is earlier than the minimum supported date.");
        }

        [Fact]
        public void Date_Should_HaveError_When_InTheFuture()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Date = DateTime.Today.AddDays(1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("Transaction date cannot be in the future.");
        }

        [Fact]
        public void Date_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Date);
        }

        // ---------- Name ----------

        [Fact]
        public void Name_Should_HaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Name = string.Empty;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Transaction name is required.");
        }

        [Fact]
        public void Name_Should_HaveError_When_ExceedsMaxLength()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Name = new string('A', TransactionConstraints.NameMaxLength + 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage($"Transaction name must not exceed {TransactionConstraints.NameMaxLength} characters.");
        }

        [Fact]
        public void Name_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        // ---------- Amount ----------

        [Fact]
        public void Amount_Should_HaveError_When_BelowMinimum()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Amount = TransactionConstraints.MinimumAmount - 1;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage($"Amount must be between {TransactionConstraints.MinimumAmount} and {TransactionConstraints.MaximumAmount}.");
        }

        [Fact]
        public void Amount_Should_HaveError_When_AboveMaximum()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Amount = TransactionConstraints.MaximumAmount + 1;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage($"Amount must be between {TransactionConstraints.MinimumAmount} and {TransactionConstraints.MaximumAmount}.");
        }

        [Fact]
        public void Amount_Should_NotHaveError_When_WithinRange()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        // ---------- Type ----------

        [Fact]
        public void Type_Should_HaveError_When_InvalidEnumValue()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Type = (TransactionType)999;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Type)
                .WithErrorMessage("Invalid transaction type.");
        }

        [Fact]
        public void Type_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Type);
        }

        // ---------- AccountId ----------

        [Fact]
        public void AccountId_Should_HaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.AccountId = Guid.Empty;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AccountId)
                .WithErrorMessage("Account ID is required.");
        }

        [Fact]
        public void AccountId_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AccountId);
        }

        // ---------- CategoryId ----------

        [Fact]
        public void CategoryId_Should_HaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.CategoryId = Guid.Empty;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("Category ID is required.");
        }

        [Fact]
        public void CategoryId_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.CategoryId);
        }
    }
}
