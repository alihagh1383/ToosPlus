namespace Web.Repo.Interface
{
    public interface ICategoriesRepo
    {
        public IEnumerable<string> GetCategories();
        public IEnumerable<CategoriesByNewsCount> GetAllData();
        public IEnumerable<string> GetCategoriesByCount();
        public void AddCategory(string category);
        public void RemoveCategory(Guid Id);
        public void UpdateCategorie(Guid Id, string NewName);



        public struct CategoriesByNewsCount
        {
            public string Name; public int NewsCount; public Guid Id;
        }
    }
}
