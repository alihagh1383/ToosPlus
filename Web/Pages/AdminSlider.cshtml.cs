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
                if (sliderRepo.GetSlide(SlideId ?? Guid.Empty) is { } s) SingelSlide = s;
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

            SingelSlide = new Slide();
            SlideId = null;
            return Page();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            sliderRepo.RemoveSlide(id);
            return Page();
        }

        public IActionResult OnPostActive(Guid id, bool active)
        {
            if (active)
            {
                sliderRepo.SetActive(id);
            }
            else sliderRepo.SetDeActive(id);
            SingelSlide = new Slide();
            return Page();
        }
    }
}