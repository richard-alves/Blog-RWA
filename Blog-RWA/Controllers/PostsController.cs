using BlogR.Entities;
using BlogR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(PostBaseModel postAddModel)
        {
            if (CurrentUser == 0)
            {
                return BadRequest("Usuário não encontrado ou não autenticado");
            }

            DateTime dateTime = DateTime.UtcNow;

            PostModel post = new()
            {
                Title = postAddModel.Title,
                Content = postAddModel.Content,
                CreatedAt = dateTime,
                LastUpdatedAt = dateTime,
                UserId = CurrentUser
            };

            var createdPost = await _postService.CreatePost(post);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, PostBaseModel postUpdateModel)
        {
            var post = await _postService.GetPostById(id);
            if (post == null) return NotFound();

            if (CurrentUser == 0) return BadRequest("Usuário não encontrado ou não autenticado");

            post.Title = postUpdateModel.Title;
            post.Content = postUpdateModel.Content;
            post.LastUpdatedAt = DateTime.UtcNow;

            var updatedPost = await _postService.UpdatePost(post, CurrentUser);
            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePost(id, CurrentUser);
            if (!result) return NotFound();
            return Ok();
        }

        private int CurrentUser
        {
            get
            {
                if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
                    return userId;
                else 
                    return 0;
            }
        }
    }
}
