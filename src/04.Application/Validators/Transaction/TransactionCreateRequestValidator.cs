using FluentValidation;
using Obscura.FinanceTracker.Application.DTOs.Transactions.Requests;
using Obscura.FinanceTracker.Shared.Constants;

namespace Obscura.FinanceTracker.Application.Validators.Transaction
{
    public class TransactionCreateRequestValidator : AbstractValidator<TransactionCreateRequest>
    {
        public TransactionCreateRequestValidator()
        {
            RuleFor(x => x.Date)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Transaction date is required.")
                .GreaterThanOrEqualTo(TransactionConstraints.MinimumDate)
                .WithMessage("Transaction date is earlier than the minimum supported date.")
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Transaction name is required.")
                .MaximumLength(TransactionConstraints.NameMaxLength)
                .WithMessage($"Transaction name must not exceed {TransactionConstraints.NameMaxLength} characters.");

            RuleFor(x => x.Amount)
                .InclusiveBetween(TransactionConstraints.MinimumAmount, TransactionConstraints.MaximumAmount)
                .WithMessage($"Amount must be between {TransactionConstraints.MinimumAmount} and {TransactionConstraints.MaximumAmount}.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid transaction type.");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Account ID is required.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category ID is required.");

            //RuleFor(x => x.Note)
            //    .MaximumLength(TransactionConstraints.NoteMaxLength)
            //    .WithMessage($"Note cannot exceed {TransactionConstraints.NoteMaxLength} characters.");

        }
    }
}
