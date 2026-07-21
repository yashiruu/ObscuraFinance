using FluentValidation.TestHelper;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;
using Obscura.FinanceTracker.Application.Validators.Category;
using Obscura.FinanceTracker.Domain.Enums;
using Obscura.FinanceTracker.Shared.Constants;
using Xunit;

namespace ObscuraFinance.Application.UnitTest.Validators.Category
{
    public class CategoryUpdateRequestValidatorTest
    {
        private readonly CategoryUpdateRequestValidator _validator;

        public CategoryUpdateRequestValidatorTest()
        {
            _validator = new CategoryUpdateRequestValidator();
        }

        private static CategoryUpdateRequest CreateValidRequest()
        {
            return new CategoryUpdateRequest
            {
                Name = "Groceries",
                Description = "Daily household groceries",
                Type = TransactionType.Expense
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
                .WithErrorMessage("Category name is required.");
        }

        [Fact]
        public void Name_Should_HaveError_When_ExceedsMaxLength()
        {
            // Arrange
            var request = CreateValidRequest();
            request.Name = new string('A', CategoryConstraints.NameMaxLength + 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage($"Category name cannot exceed {CategoryConstraints.NameMaxLength} characters.");
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
            request.Description = new string('A', CategoryConstraints.DescriptionMaxLength + 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage($"Description cannot exceed {CategoryConstraints.DescriptionMaxLength} characters.");
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
                .WithErrorMessage("Invalid category type.");
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
