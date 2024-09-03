using Blog_RWA.Entities;

namespace Blog_RWA.Services.Implementation
{
    public interface IPostService
    {
        Task<IEnumerable<PostModel>> GetAllPosts();
        Task<PostModel>? GetPostById(int id);
        Task<PostModel> CreatePost(PostModel post);
        Task<PostModel> UpdatePost(PostModel post, int userId);
        Task<bool> DeletePost(int id, int userId);
    }
}
