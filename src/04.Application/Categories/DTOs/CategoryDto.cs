namespace Obscura.FinanceTracker.Application.Categories.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int Type { get; set; }
    }
}
