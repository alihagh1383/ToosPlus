using System.ComponentModel.DataAnnotations;

namespace Web.DTO.Write
{
    public class WNewsDto
    {
        [Required][MaxLength(500)] public string Title { get; set; } = null!;
        [Required] public string Content { get; set; } = null!;
        [Required] public DateOnly? Date { get; set; }
        public string? Creator { get; set; } = null!;
        [Required] public string Category { get; set; } = null!;
        public string? TitleImageName { get; set; }
        public string Description { get; set; } = null!;
    }
}
