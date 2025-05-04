using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web.DTO.Read;
using Web.Repo.Interface;

namespace Web.Pages
{
    public class IndexModel(INewsRepo newsRepo) : PageModel
    {
        public List<RNewsDto> News { get; set; } = [];
        public int Pages { get; set; } = 0;
        [FromQuery] public int Count { get; set; } = 48;
        [FromQuery] public int PageIndex { get; set; } = 0;
        public void OnGet()
        {
            var news = newsRepo.GetNewsWithCreatorAndCategory(Count, PageIndex);
            News = [.. news.news];
            Pages = news.countall;
        }
    }
}
