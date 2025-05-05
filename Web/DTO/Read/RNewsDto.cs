namespace Web.DTO.Read
{
    public class RNewsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateOnly Date { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? TitleImageName { get; set; }
    }
}
