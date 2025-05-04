using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.DTO.Read;
using Web.Repo.Interface;

namespace Web.Pages
{
    public class NewsModel(INewsRepo newsRepo) : PageModel
    {
        [FromQuery] public Guid NewsId { get; set; }
        public RNewsDto RNewsDto { get; set; } = null!;
        public void OnGet()
        {
            if (newsRepo.GetNews(NewsId).news is { } u)
            {
                RNewsDto = u;
            }
            else HttpContext.Response.Redirect("/");
        }
    }
}
