using BlogR.Entities;

namespace BlogR.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<IEnumerable<UserModel>> GetAllUsers();
    }
}
