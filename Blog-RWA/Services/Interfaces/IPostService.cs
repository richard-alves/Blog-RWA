using BlogR.Entities;

namespace BlogR.Services.Interfaces
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
