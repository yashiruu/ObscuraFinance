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
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(CategoryConstraints.NameMaxLength)
                .WithMessage($"Category name cannot exceed {CategoryConstraints.NameMaxLength} characters.");


            RuleFor(c => c.Description)
                .MaximumLength(CategoryConstraints.DescriptionMaxLength)
                .WithMessage($"Description cannot exceed {CategoryConstraints.DescriptionMaxLength} characters."); ;

            RuleFor(c => c.Type)
                .IsInEnum()
                .WithMessage("Invalid category type."); ;
        }
    }
}
