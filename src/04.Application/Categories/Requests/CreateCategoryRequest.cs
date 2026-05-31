namespace Obscura.FinanceTracker.Application.Categories.Requests
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; }
    }
}
