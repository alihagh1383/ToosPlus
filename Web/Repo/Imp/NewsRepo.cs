using Data;
using Microsoft.EntityFrameworkCore;
using Web.DTO.Read;
using Web.DTO.Write;
using Web.Repo.Interface;

namespace Web.Repo.Imp
{
    public class NewsRepo(NewsDbContext dbContext) : INewsRepo
    {
        public (RNewsDto? news, bool Sucsses) AddNews(WNewsDto dto)
        {
            var c = dbContext.NewsCategories.FirstOrDefault(c => c.Name == dto.Category);
            var u = dbContext.Users.FirstOrDefault(u => u.Username == dto.Creator);
            if (c != null && u != null)
            {
                var n = dbContext.News.Add(new News
                {
                    Category = c,
                    Creator = u,
                    Content = dto.Content,
                    Title = dto.Title,
                    Date = dto.Date ?? new DateOnly(),
                    TitleImageName = dto.TitleImageName
                }).Entity;
                dbContext.SaveChanges();
                return (new RNewsDto
                {
                    Author = u.Username,
                    Category = c.Name,
                    Content = dto.Content,
                    Title = dto.Title,
                    Date = dto.Date ?? new DateOnly(),
                    Id = n.Id,
                    TitleImageName = dto.TitleImageName
                }, true);
            }
            else
            {
                return (null, false);
            }
        }

        public (RNewsDto? news, bool Sucsses) GetNews(Guid id)
        {
            return dbContext.News.AsNoTracking()
                .Include(p => p.Creator)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id) is { } n
                ? (new RNewsDto { Title = n.Title, Id = n.Id, Author = n.Creator.Username, Category = n.Category.Name, Content = n.Content, Date = n.Date, TitleImageName = n.TitleImageName }, true)
                : (null, false);
        }

        public (IEnumerable<RNewsDto> news, int countall) GetNewsWithCreatorAndCategory(int count, int page, string? cat)
        {
            var linq = dbContext.News
                    .Where(p => cat == null || p.Category.Name == cat);
            var list =
                linq
                    .Select(p => new { p.Category, p.Creator, p.Content, p.Date, p.Title, p.Id, p.TitleImageName })
                    .OrderBy(p => p.Date)
                    .Skip(page * count)
                    .Take(count)
                    .Select(p => new RNewsDto
                    {
                        Id = p.Id,
                        Date = p.Date,
                        Title = p.Title,
                        Author = p.Creator.Username,
                        Content = p.Content,
                        Category = p.Category.Name,
                        TitleImageName = p.TitleImageName
                    });
            return (list, linq.Count() / count);
        }

        public (IEnumerable<RNewsDto> news, int countall) SearchNewsWithCreatorAndCategory(int count, int page, string q, string? cat = null)
        {
            var linq = dbContext.News
                    .Where(p => cat == null || p.Category.Name == cat)
                    .Where(p => p.Title.Contains(q));
            var list =
                linq
                    .Select(p => new { p.Category, p.Creator, p.Content, p.Date, p.Title, p.Id, p.TitleImageName })
                    .OrderBy(p => p.Date)
                    .Skip(page * count)
                    .Take(count)
                    .Select(p => new RNewsDto
                    {
                        Id = p.Id,
                        Date = p.Date,
                        Title = p.Title,
                        Author = p.Creator.Username,
                        Content = p.Content,
                        Category = p.Category.Name,
                        TitleImageName = p.TitleImageName
                    });
            return (list, linq.Count() / count);
        }

        public (RNewsDto? news, bool Sucsses) UpdateNews(RNewsDto dto)
        {
            if (dbContext.News.AsNoTracking().FirstOrDefault(p => p.Id == dto.Id) is not { } n
                || dbContext.Users.AsNoTracking().FirstOrDefault(p => p.Username == dto.Author) is not { } u
                || dbContext.NewsCategories.AsNoTracking().FirstOrDefault(p => p.Name == dto.Category) is not { } c)
                return (null, false);
            n.Title = dto.Title;
            n.Date = dto.Date;
            n.Creator = u;
            n.Content = dto.Content;
            n.Category = c;
            n.TitleImageName = dto.TitleImageName ?? n.TitleImageName;
            dbContext.News.Update(n);
            dbContext.SaveChanges();
            return (new RNewsDto
            {
                Id = n.Id,
                Date = n.Date,
                Title = n.Title,
                Author = u.Username,
                Content = n.Content,
                Category = c.Name,
                TitleImageName = n.TitleImageName
            }, true);
        }
    }
}
