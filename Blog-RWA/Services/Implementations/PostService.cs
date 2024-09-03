using BlogR.Data;
using BlogR.Entities;
using BlogR.Hubs;
using BlogR.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlogR.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly BlogDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PostService(BlogDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<PostModel>> GetAllPosts()
        {
            return await _context.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<PostModel>? GetPostById(int id)
        {
            return await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PostModel> CreatePost(PostModel post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            post.User = _context.Users.Find(post.UserId);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", post.User.Username, post.Title);
            return post;
        }

        public async Task<PostModel> UpdatePost(PostModel post, int userId)
        {
            ValidateUser(userId, post.UserId);

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePost(int id, int userId)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return false;

            ValidateUser(userId, post.UserId);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        private void ValidateUser(int userId, int originalUserId)
        {
            if (userId != originalUserId)
            {
                throw new ArgumentException("Você só pode alterar e remover posts criado por você.");
            }
        }
    }
}
