namespace Obscura.FinanceTracker.Application.Categories.Requests
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int Type { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdateBy { get; set; }
    }
}
