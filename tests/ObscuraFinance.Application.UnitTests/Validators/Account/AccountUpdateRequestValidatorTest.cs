using FluentValidation.TestHelper;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Application.Validators.Account;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Constants;
using Xunit;

namespace ObscuraFinance.Application.UnitTest.Validators.Account
{
    public class AccountUpdateRequestValidatorTest
    {
        private readonly AccountUpdateRequestValidator _validator;

        public AccountUpdateRequestValidatorTest()
        {
            _validator = new AccountUpdateRequestValidator();
        }

        private static AccountUpdateRequest CreateValidRequest()
        {
            return new AccountUpdateRequest
            {
                Name = "Cash Wallet",
                Description = "My daily cash wallet",
                Currency = "USD",
                Type = AccountType.Cash
            };
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
                .WithErrorMessage("Account name is required.");
        }

        [Fact]
        public void Name_Should_HaveError_When_ExceedsMaxLength()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Name = new string('A', AccountConstraints.NameMaxLength + 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage($"Account name must not exceed {AccountConstraints.NameMaxLength} characters.");
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

        // ---------- Description ----------

        [Fact]
        public void Description_Should_HaveError_When_ExceedsMaxLength()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Description = new string('A', AccountConstraints.DescriptionMaxLength + 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage($"Account description must not exceed {AccountConstraints.DescriptionMaxLength} characters.");
        }

        [Fact]
        public void Description_Should_NotHaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Description = string.Empty;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        // ---------- Currency ----------

        [Fact]
        public void Currency_Should_HaveError_When_Empty()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Currency = string.Empty;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Currency is required.");
        }

        [Fact]
        public void Currency_Should_HaveError_When_LengthIsInvalid()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Currency = "US";

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage($"Currency must be {AccountConstraints.CurrencyLength} characters long.");
        }

        [Fact]
        public void Currency_Should_HaveError_When_FormatIsInvalid()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Currency = "usd";

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Currency)
                .WithErrorMessage("Currency must be a valid ISO 4217 currency code.");
        }

        [Fact]
        public void Currency_Should_NotHaveError_When_Valid()
        {
            // Arrange
            var request = CreateValidRequest();

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Currency);
        }

        // ---------- Type ----------

        [Fact]
        public void Type_Should_HaveError_When_InvalidEnumValue()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Type = (AccountType)999;

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Type)
                .WithErrorMessage("Invalid account type.");
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
    }
}