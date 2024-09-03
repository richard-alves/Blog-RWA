using BlogR.Entities;

namespace BlogR.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(UserModel user, string password);
        Task<string> Login(string username, string password);
    }
}
