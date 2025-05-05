using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MrX.Web.Extension;
using System.Security.Claims;
using Web.Repo.Interface;

namespace Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [FromQuery] public string? ReturnUrl { get; set; }
        [FromForm] public string Username { get; set; } = string.Empty;
        [FromForm] public string Password { get; set; } = string.Empty;
        [FromForm] public bool RememberMe { get; set; } = false;
        public void OnGet()
        {
        }
        public async Task OnPost([FromServices] IUserRepo userRepo)
        {
            if (userRepo.GetUser(Username) is { } u && u.VeryfiPassword(Password))
            {
                await HttpContext.SignOutAsync();

                List<Claim> claims = [new Claim(ClaimTypes.Name, Username)];
                Console.WriteLine(nameof(UserRule.Admin));
                if (u.Rule.HasFlag(UserRule.Admin)) claims.Add(new Claim(ClaimTypes.Role, nameof(UserRule.Admin)));
                else if (u.Rule.HasFlag(UserRule.Editor)) claims.Add(new Claim(ClaimTypes.Role, nameof(UserRule.Editor)));
                else if (u.Rule.HasFlag(UserRule.None)) claims.Add(new Claim(ClaimTypes.Role, nameof(UserRule.None)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(10)
                        : DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = RememberMe,
                    AllowRefresh = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    RedirectUri = ReturnUrl ?? "/",
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                TempData.SetOk("ورود شما با موفقیت انجام شد");
            }
            else TempData.SetError("نام کاربری یا رمزعبور درست نیست");
        }
    }
}
