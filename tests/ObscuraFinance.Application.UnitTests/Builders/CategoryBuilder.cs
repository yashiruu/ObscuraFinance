using Obscura.FinanceTracker.Domain.Entities;
using Obscura.FinanceTracker.Domain.Enums;

namespace ObscraFinance.Application.UnitTests.Builders
{
    public sealed class CategoryBuilder
    {
        private readonly Category _category;

        private CategoryBuilder()
        {
            _category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Default Category",
                Description = "Default Description",
                Type = TransactionType.Expense
            };
        }

        public static CategoryBuilder Create()
        {
            return new CategoryBuilder();
        }

        public CategoryBuilder WithId(Guid id)
        {
            _category.Id = id;
            return this;
        }

        public CategoryBuilder WithName(string name)
        {
            _category.Name = name;
            return this;
        }

        public CategoryBuilder WithDescription(string description)
        {
            _category.Description = description;
            return this;
        }

        public CategoryBuilder WithType(TransactionType type)
        {
            _category.Type = type;
            return this;
        }

        public CategoryBuilder AsDeleted()
        {
            _category.IsDeleted = true;
            _category.DeletedAt = DateTime.UtcNow;
            return this;
        }

        public Category Build()
        {
            return _category;
        }
    }
}