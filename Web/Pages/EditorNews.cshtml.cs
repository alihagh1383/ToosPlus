using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using Web.DTO.Read;
using Web.DTO.Write;
using Web.Repo.Interface;

namespace Web.Pages
{
    [Authorize(policy: "Editor")]
    public class EditorNewsModel(INewsRepo newsRepo, ICategoriesRepo categoriesRepo) : PageModel
    {
        [FromQuery] public Guid? NewsId { get; set; }
        [FromForm] public WNewsDto News { get; set; } = new WNewsDto();
        public List<string> Categories { get; set; } = [.. categoriesRepo.GetCategories()];
        public IActionResult OnGet()
        {
            if (NewsId is not null) if (newsRepo.GetNews(NewsId ?? new()).news is { } n &&
                (n.Author == HttpContext.User.FindFirstValue(ClaimTypes.Name)
                || HttpContext.User.IsInRole(nameof(UserRule.Admin))
                || HttpContext.User.IsInRole(nameof(UserRule.Developer))))
                {
                    News.Content = n.Content;
                    News.Title = n.Title;
                    News.Creator = n.Author;
                    News.Category = n.Category;
                    News.Date = n.Date;
                    News.TitleImageName = n.TitleImageName;
                    return Page();
                }
                else return RedirectToPage("/News", new { area = "Editor" });
            else return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            switch (NewsId)
            {
                case null:
                    {
                        News.Creator = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Name).Value;
                        var r = newsRepo.AddNews(News);
                        if (!r.Sucsses) return Page();
                        else
                        {
                            HttpContext.Response.StatusCode = StatusCodes.Status201Created;
                            return RedirectToPage("/News", new { area = "Editor", NewsId = r.news!.Id });
                        }
                    }
                case { }:
                    {
                        var n = newsRepo.GetNews((Guid)NewsId);
                        if (n.Sucsses)
                            if (n.news!.Author == HttpContext.User.FindFirstValue(ClaimTypes.Name)
                                || HttpContext.User.IsInRole(nameof(UserRule.Admin))
                                || HttpContext.User.IsInRole(nameof(UserRule.Developer)))
                            {
                                newsRepo.UpdateNews(new()
                                {
                                    Author = n.news.Author,
                                    Category = News.Category,
                                    Content = News.Content,
                                    Date = News.Date ?? n.news.Date,
                                    Id = n.news.Id,
                                    Title = News.Title,
                                    TitleImageName = News.TitleImageName ?? n.news.TitleImageName
                                });
                                return RedirectToPage("/News", new { area = "Editor", NewsId = n.news.Id });
                            }
                        return RedirectToPage("/News", new { area = "Editor" });

                    }
            }

        }
    }
}
