using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class News
    {
        [Key] public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)][MaxLength(500)] public string Title { get; set; } = null!;
        [Required(AllowEmptyStrings = false)] public string Content { get; set; } = string.Empty;
        public List<User> Likes { get; set; } = [];
        [Required] public User Creator { get; set; } = null!;
        [Required] public NewsCategory Category { get; set; } = null!;
        [Required][Editable(false)] public DateTime CreateAt { get; set; } = DateTime.Now;
        [Required] public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? TitleImageName { get; set; }
    }
}
