using FluentValidation;
using Obscura.FinanceTracker.Shared.Constants;
using Obscura.FinanceTracker.Application.DTOs.Categories.Requests;

namespace Obscura.FinanceTracker.Application.Validators.Category
{
    public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(CategoryConstants.NameMaxLength);

            RuleFor(c => c.Description)
                .MaximumLength(CategoryConstants.DescriptionMaxLength);

            RuleFor(c => c.Type)
                .IsInEnum();
        }
    }
}
