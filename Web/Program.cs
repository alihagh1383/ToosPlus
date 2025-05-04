using Data;
using Google.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MrX.Web.Security;
using Web.Repo.Imp;
using Web.Repo.Interface;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Services.AddSingleton<FileService>(p => new("Uploads"));
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(name: "Developer"/**/, configurePolicy: p => p.RequireRole(nameof(UserRule.Developer)))
    .AddPolicy(name: "Admin"/******/, configurePolicy: p => p.RequireRole(nameof(UserRule.Developer), nameof(UserRule.Admin)))
    .AddPolicy(name: "Editor"/*****/, configurePolicy: p => p.RequireRole(nameof(UserRule.Developer), nameof(UserRule.Admin), nameof(UserRule.Editor)));
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Login";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ClaimsIssuer = "MrX";
    });
builder.Services.AddDbContextPool<NewsDbContext>(o =>
    _ = builder.Configuration.GetValue<string>("Provider")!.ToLower() switch
    {
        "sqlite" => o.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"), b => b.MigrationsAssembly("Sqlite")),
        "sqlserver" => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), b => b.MigrationsAssembly("SqlServer")),
        _ => throw new InvalidDataException("Database Provider Not Suport")
    });
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<INewsRepo, NewsRepo>();
builder.Services.AddScoped<ISliderRepo, SliderRepo>();
builder.Services.AddScoped<ICategoriesRepo, CategoriesRepo>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapGet("/File/{name}", FileService.DownloadFile);
app.MapBlazorHub();
{
    char[] fa = "ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوًٌٍَُِّۀآةيژؤإأء؟.     ".ToArray();
    var db = app.Services.CreateScope().ServiceProvider.GetService<NewsDbContext>()!;
    db.Database.Migrate();
    if (!db.Users.Any())
        db.Users.Add(new User() { Username = "basig", Rule = UserRule.Admin }.SetPassword("basig123456@@"));
    if (!db.NewsCategories.Any())
        db.NewsCategories.AddRange(Enumerable.Range(0, 5).Select(p => new NewsCategory() { Name = MrX.Web.Security.Random.String(10, lowerCase: true, lowerCaseSet: fa) }));
    db.SaveChanges();
    if (!db.News.Any())
        db.News.AddRange(Enumerable.Range(0, 100).Select(p => new News()
        {
            Content = MrX.Web.Security.Random.String(100000, lowerCase: true, lowerCaseSet: fa),
            Title = MrX.Web.Security.Random.String(100, lowerCase: true, lowerCaseSet: fa),
            Creator = db.Users.First(),
            Category = db.NewsCategories.AsEnumerable().OrderBy(p => Guid.NewGuid()).First(),
            TitleImageName = "019696c0-6bd5-71fb-8b72-b869ef0658fe.jpg"
        }));
    db.SaveChanges();
    db.Dispose();
}
app.Run();
