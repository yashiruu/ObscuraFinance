namespace Obscura.FinanceTracker.Shared.Constants
{
    public static class AccountConstraints
    {
        public const int NameMaxLength = 100;

        public const int DescriptionMaxLength = 500;
        
        public const int CurrencyLength = 3;

        public const decimal MinimumInitialBalance = 0m;

        public const decimal MaximumInitialBalance = 999_999_999_999m;
    }
}
