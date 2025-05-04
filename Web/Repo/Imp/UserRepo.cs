using Data;
using Microsoft.EntityFrameworkCore;
using Web.Repo.Interface;

namespace Web.Repo.Imp
{
    public class UserRepo(NewsDbContext dbContext) : IUserRepo
    {
        public User? GetUser(string username)
        {
            username = username.ToLower();
            return dbContext.Users.AsNoTracking().FirstOrDefault(p => p.Username == username);
        }
    }
}
