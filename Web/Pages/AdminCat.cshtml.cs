using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MrX.Web.Extension;
using Web.Repo.Interface;

namespace Web.Pages
{
    [Authorize(policy: "Admin")]
    public class AdminCatModel(ICategoriesRepo categoriesRepo) : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                TempData.SetError("نام نباید خالی باشد");
                return;
            }
            categoriesRepo.AddCategory(Name);
        }
        public void OnPostPut(Guid Id, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                TempData.SetError("نام نباید خالی باشد");
                return;
            }
            categoriesRepo.UpdateCategorie(Id, Name);
        }
        public void OnPostDelete(Guid Id)
        {
            categoriesRepo.RemoveCategory(Id);
        }
    }
}
