using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.DTO.Read;
using Web.Repo.Interface;

namespace Web.Pages
{
    public class SearchModel(INewsRepo newsRepo) : PageModel
    {
        [FromQuery] public string? Cat { get; set; }
        [FromQuery] public string? Q { get; set; }
        [FromQuery] public int PageIndex { get; set; } = 0;
        [FromQuery] public int Count { get; set; } = 50;
        public List<RNewsDto> News { get; set; } = [];
        public int Pages { get; set; } = 0;
        public void OnGet()
        {
            var news = !string.IsNullOrWhiteSpace(Q)
                  ? newsRepo.SearchNewsWithCreatorAndCategory(Count, PageIndex, Q, Cat)
                  : newsRepo.GetNewsWithCreatorAndCategory(Count, PageIndex, Cat);
            News = [.. news.news];
            Pages = news.countall;
        }
    }
}
