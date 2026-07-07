using FluentValidation;
using Obscura.FinanceTracker.Application.Accounts.DTOs;
using Obscura.FinanceTracker.Shared.Constants;

namespace Obscura.FinanceTracker.Application.Validators.Account
{
    public class AccountUpdateRequestValidator : AbstractValidator<AccountUpdateRequest>
    {
        public AccountUpdateRequestValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                    .WithMessage("Account ID is required.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Account name is required.")
                .MaximumLength(AccountConstraints.NameMaxLength)
                    .WithMessage($"Account name must not exceed {AccountConstraints.NameMaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(AccountConstraints.DescriptionMaxLength)
                .WithMessage($"Account description must not exceed {AccountConstraints.DescriptionMaxLength} characters.");

            RuleFor(x => x.Currency)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Currency is required.")
                .Length(AccountConstraints.CurrencyLength)
                    .WithMessage($"Currency must be {AccountConstraints.CurrencyLength} characters long.")
                .Matches("^[A-Z]{3}$")
                    .WithMessage("Currency must be a valid ISO 4217 currency code."); ;

            RuleFor(x => x.Type)
                .IsInEnum()
                    .WithMessage("Invalid account type.");
        }
    }
}
