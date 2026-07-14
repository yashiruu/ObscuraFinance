using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace ObscuraFinance.Application.UnitTests.Builders
{
    public sealed class AccountBuilder
    {
        private readonly Account _account;

        private AccountBuilder()
        {
            _account = new Account
            {
                Id = Guid.NewGuid(),
                Name = "Default Account",
                Description = "Default Description",
                InitialBalance = 0m,
                CurrentBalance = 0m,
                Currency = "IDR",
                Type = AccountType.Bank,
                IsActive = true,
            };
        }

        public static AccountBuilder Create()
        {
            return new AccountBuilder();
        }

        public AccountBuilder WithId(Guid id)
        {
            _account.Id = id;
            return this;
        }

        public AccountBuilder WithName(string name)
        {
            _account.Name = name;
            return this;
        }

        public AccountBuilder WithDescription(string description)
        {
            _account.Description = description;
            return this;
        }

        public AccountBuilder WithInitialBalance(decimal initialBalance)
        {
            _account.InitialBalance = initialBalance;
            return this;
        }

        public AccountBuilder WithCurrentBalance(decimal currentBalance)
        {
            _account.CurrentBalance = currentBalance;
            return this;
        }

        public AccountBuilder WithCurrency(String currency)
        {
            _account.Currency = currency;
            return this;
        }

        public AccountBuilder WithType(AccountType type)
        {
            _account.Type = type;
            return this;
        }

        public AccountBuilder AsInactive()
        {
            _account.IsActive = false;
            return this;
        }

        public AccountBuilder AsDeleted()
        {
            _account.IsActive = true;
            _account.DeletedAt = DateTime.UtcNow;
            return this;
        }

        public Account Build()
        {
            return _account;
        }
    }
}