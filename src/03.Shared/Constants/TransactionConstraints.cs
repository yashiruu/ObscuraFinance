namespace Obscura.FinanceTracker.Shared.Constants
{
    public static class TransactionConstraints
    {
        public const int NameMaxLength = 100;

        public const decimal MinimumAmount = 0.01m;

        public const decimal MaximumAmount = 999_999_999_999m;

        public const int NoteMaxLength = 1000;

        public static readonly DateTime MinimumDate = new (2000, 1, 1);
    }
}
