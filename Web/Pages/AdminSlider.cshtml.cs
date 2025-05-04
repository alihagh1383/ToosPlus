using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Repo.Interface;

namespace Web.Pages
{
    [Authorize("Admin")]
    public class AdminSliderModel(ISliderRepo sliderRepo) : PageModel
    {
        [FromQuery] public Guid? SlideId { get; set; }
        public IEnumerable<Slide> Slides { get; set; } = sliderRepo.GetAllSlides().OrderBy(p => p.IsActive);
        [FromForm] public Slide SingelSlide { get; set; } = new();
        public IActionResult OnGet()
        {
            if (SlideId is not null)
            {
                if (sliderRepo.GetSlide(SlideId ?? new()) is { } s) SingelSlide = s;
                else return RedirectToPage("AdminSlider");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            if (SlideId is not null)
            {
                SingelSlide.Id = SlideId ?? SingelSlide.Id;
                sliderRepo.UpdateSlide(SingelSlide);
            }
            else
            {
                sliderRepo.AddSlide(SingelSlide);
            }
            return Page();
        }
        public IActionResult OnPostDelete(Guid Id)
        {
            sliderRepo.RemoveSlide(Id);
            return Page();
        }
        public IActionResult OnPostActive(Guid Id, bool Active)
        {
            if (Active) { sliderRepo.SetActive(Id); }
            else sliderRepo.SetDeActive(Id);
            return Page();
        }
    }
}
