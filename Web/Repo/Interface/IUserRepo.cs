using Data;

namespace Web.Repo.Interface
{
    public interface IUserRepo
    {
        public User? GetUser(string username);
    }
}
