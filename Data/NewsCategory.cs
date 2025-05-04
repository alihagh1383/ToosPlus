using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class NewsCategory
    {
        [Key] public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)] public string Name { get; set; } = null!;
        public List<News> News { get; set; } = [];
    }
}
