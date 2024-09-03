using Blog_RWA.Entities;

namespace Blog_RWA.Services.Implementation
{
    public interface IAuthService
    {
        Task<string> Register(UserModel user, string password);
        Task<string> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<IEnumerable<UserModel>> GetAllUsers();
    }
}
