using BlogR.Entities;
using BlogR.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string userName, string password)
        {
            try
            {
                var user = new UserModel { Username = userName };
                var token = await _authService.Register(user, password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            try
            {
                var token = await _authService.Login(userName, password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}