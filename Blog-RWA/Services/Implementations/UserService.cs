using BlogR.Data;
using BlogR.Entities;
using BlogR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogR.Services.Implementations
{
    public class UserService:IUserService
    {
        private readonly BlogDbContext _context;

        public UserService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
