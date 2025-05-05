using Data;

namespace Web.Repo.Interface
{
    public interface ICategoriesRepo
    {
        public IEnumerable<string> GetCategories();
        public IEnumerable<CategoriesByNewsCount> GetAllData();
        public IEnumerable<string> GetCategoriesByCount();
        public void AddCategory(string category);
        public void RemoveCategory(Guid id);
        public void UpdateCategorieName(Guid id, string newName);
        public void UpdateCategorieLoc(Guid id, LocationInIndex newName);
        public NewsCategory? GetCategoryById(Guid id);
        public NewsCategory? GetCategoryByIdWhitNews(Guid id, int? count = null);
        public CategoriesByNewsCount? GetCategoriesByLocationInIndex(LocationInIndex location);

        public struct CategoriesByNewsCount
        {
            public string Name; public int NewsCount; public Guid Id;public LocationInIndex LocationInIndex;
        }
    }
}
