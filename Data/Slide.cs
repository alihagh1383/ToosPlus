using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Slide
    {
        [Key] public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)] public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Required] public bool IsActive { get; set; } = false;
    }
}
