using FluentValidation;
using Obscura.FinanceTracker.Application.DTOs.Accounts.Requests;
using Obscura.FinanceTracker.Shared.Constants;

namespace Obscura.FinanceTracker.Application.Validators.Account
{
    public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateRequestValidator() 
        {
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
                .WithMessage($"Currency must be {AccountConstraints.CurrencyLength} characters long.");

            RuleFor(x => x.InitialBalance)
                .InclusiveBetween(AccountConstraints.MinimumInitialBalance, AccountConstraints.MaximumInitialBalance)
                .WithMessage($"Initial balance must be between {AccountConstraints.MinimumInitialBalance} and {AccountConstraints.MaximumInitialBalance}.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid account type.");
        }

    }
}
