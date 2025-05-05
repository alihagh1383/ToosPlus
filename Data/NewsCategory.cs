using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class NewsCategory
    {
        [Key] public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)] public string Name { get; set; } = null!;
        public List<News> News { get; set; } = [];
        public LocationInIndex? LocationInIndex { get; set; } = Data.LocationInIndex.None;
    }

    public enum LocationInIndex
    {
        [Display(Name = "نمایش نده")] None = 0b0000000001,
        [Display(Name = "راست اسلایدر")] SliderR = 0b0000000010,
        [Display(Name = "چپ اسلایدر")] SliderL = 0b0000000100,
        [Display(Name = "پایین اسلایدر")] BottomSliderR = 0b0000001000,
        [Display(Name = "چپ اسلایدر پایین")] BottomSlider = 0b0000010000,
    }
}