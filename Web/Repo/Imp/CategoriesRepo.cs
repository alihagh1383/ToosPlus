using Data;
using Microsoft.EntityFrameworkCore;
using MrX.Web.ApiCallback;
using Web.Repo.Interface;
using static Web.Repo.Interface.ICategoriesRepo;

namespace Web.Repo.Imp
{
    public class CategoriesRepo(NewsDbContext dbContext) : ICategoriesRepo
    {
        public void AddCategory(string category)
        {
            dbContext.NewsCategories.Add(new()
            {
                Name = category,
            });
            dbContext.SaveChanges();
        }

        public IEnumerable<ICategoriesRepo.CategoriesByNewsCount> GetAllData()
        {
            return dbContext.NewsCategories.Select(c => new CategoriesByNewsCount() { Name = c.Name, Id = c.Id, NewsCount = c.News.Count });
        }

        public IEnumerable<string> GetCategories()
        {
            return dbContext.NewsCategories.Select(c => c.Name);
        }

        public IEnumerable<string> GetCategoriesByCount()
        {
            return dbContext.NewsCategories.OrderBy(p => p.News.Count).Select(c => c.Name);
        }

        public void RemoveCategory(Guid Id)
        {
            dbContext.NewsCategories.Where(p => p.Id == Id).ExecuteDelete();
        }

        public void UpdateCategorie(Guid Id, string NewName)
        {
            dbContext.NewsCategories.Where(p => p.Id == Id).ExecuteUpdate(p => p.SetProperty(p => p.Name, NewName));
        }
    }
}
