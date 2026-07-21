using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace ObscuraFinance.Application.UnitTests.Builders
{
    public sealed class TransactionBuilder
    {
        private readonly Transaction _transaction;

        private TransactionBuilder()
        {
            _transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Name = "Default Transaction",
                Amount = 100_000m,
                Type = TransactionType.Expense,
                AccountId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
            };
        }

        public static TransactionBuilder Create()
        {
            return new TransactionBuilder();
        }

        public TransactionBuilder WithId(Guid id)
        {
            _transaction.Id = id;
            return this;
        }

        public TransactionBuilder WithDate(DateTime date)
        {
            _transaction.Date = date;
            return this;
        }

        public TransactionBuilder WithName(String name)
        {
            _transaction.Name = name;
            return this;
        }

        public TransactionBuilder WithAmount(decimal amount)
        {
            _transaction.Amount = amount;
            return this;
        }

        public TransactionBuilder WithType(TransactionType type)
        {
            _transaction.Type = type;
            return this;
        }

        public TransactionBuilder WithAccountId(Guid accountId)
        {
            _transaction.AccountId = accountId;
            return this;
        }

        public TransactionBuilder WithCategoryId(Guid categoryId)
        {
            _transaction.CategoryId = categoryId;
            return this;
        }

        public TransactionBuilder WithAccount(Account account)
        {
            _transaction.AccountId = account.Id;
            _transaction.Account = account;
            return this;
        }

        public TransactionBuilder WithCategory(Category category)
        {
            _transaction.CategoryId = category.Id;
            _transaction.Category = category;
            return this;
        }

        public TransactionBuilder AsExpense()
        {
            _transaction.Type = TransactionType.Expense;
            return this;
        }

        public TransactionBuilder AsIncome()
        {
            _transaction.Type = TransactionType.Income;
            return this;
        }

        public TransactionBuilder AsDeleted()
        {
            _transaction.IsDeleted = true;
            _transaction.DeletedAt = DateTime.UtcNow;
            return this;
        }

        public Transaction Build()
        {
            return _transaction;
        }
    }
}