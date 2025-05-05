using Data;
using Microsoft.EntityFrameworkCore;
using Web.Repo.Interface;
using static Web.Repo.Interface.ICategoriesRepo;

namespace Web.Repo.Imp
{
    public class CategoriesRepo(NewsDbContext dbContext) : ICategoriesRepo
    {
        public void AddCategory(string category)
        {
            dbContext.NewsCategories.Add(new NewsCategory
            {
                Name = category,
            });
            dbContext.SaveChanges();
        }

        public IEnumerable<ICategoriesRepo.CategoriesByNewsCount> GetAllData()
        {
            return dbContext.NewsCategories.Select(c => new CategoriesByNewsCount()
            { Name = c.Name, Id = c.Id, NewsCount = c.News.Count, LocationInIndex = c.LocationInIndex ?? LocationInIndex.None });
        }

        public IEnumerable<string> GetCategories()
        {
            return dbContext.NewsCategories.Select(c => c.Name);
        }

        public IEnumerable<string> GetCategoriesByCount()
        {
            return dbContext.NewsCategories.OrderBy(p => p.News.Count).Select(c => c.Name);
        }

        public void RemoveCategory(Guid id)
        {
            dbContext.NewsCategories.Where(p => p.Id == id).ExecuteDelete();
        }

        public void UpdateCategorieName(Guid id, string newName)
        {
            dbContext.NewsCategories.Where(p => p.Id == id).ExecuteUpdate(p => p.SetProperty(c => c.Name, newName));
        }

        public void UpdateCategorieLoc(Guid id, LocationInIndex location)
        {
            if (dbContext.NewsCategories.Where(p => p.Id == id).ExecuteUpdate(p => p.SetProperty(c => c.LocationInIndex, location)) != 0)
                dbContext.NewsCategories.Where(p => p.LocationInIndex == location && p.Id != id).ExecuteUpdate(p => p.SetProperty(c => c.LocationInIndex, LocationInIndex.None));
        }

        public NewsCategory? GetCategoryById(Guid id)
        {
            return dbContext.NewsCategories.FirstOrDefault(p => p.Id == id);
        }

        public NewsCategory? GetCategoryByIdWhitNews(Guid id, int? count = null)
        {
            if (count is not null)
                return dbContext.NewsCategories.Include(p => p.News).Select(p => new NewsCategory
                {
                    Name = p.Name,
                    LocationInIndex = p.LocationInIndex,
                    Id = p.Id,
                    News = p.News.Take(count ?? 0).ToList(),
                }).FirstOrDefault(p => p.Id == id);
            return dbContext.NewsCategories.Include(p => p.News).FirstOrDefault(p => p.Id == id);
        }

        public CategoriesByNewsCount? GetCategoriesByLocationInIndex(LocationInIndex location)
        {
            return dbContext.NewsCategories.Where(p => p.LocationInIndex == location)
                .Select(c => new CategoriesByNewsCount()
                { Name = c.Name, Id = c.Id, NewsCount = c.News.Count, LocationInIndex = c.LocationInIndex ?? LocationInIndex.None })
                .FirstOrDefault();
        }
    }
}