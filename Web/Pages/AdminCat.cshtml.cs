using Data;
using Microsoft.AspNetCore.Authorization;
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

        public void OnPost(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.SetError("نام نباید خالی باشد");
                return;
            }

            categoriesRepo.AddCategory(name);
        }

        public void OnPostPut(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData.SetError("نام نباید خالی باشد");
                return;
            }
            categoriesRepo.UpdateCategorieName(id, name);
        }

        public void OnPostDelete(Guid id)
        {
            categoriesRepo.RemoveCategory(id);
        }

        public void OnPostLoc(Guid id, LocationInIndex loc)
        {
            categoriesRepo.UpdateCategorieLoc(id, loc);
        }
    }
}