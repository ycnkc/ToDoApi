using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Services;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, AuthService authService, IConfiguration configuration)
        {
            _context = context;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            user.PasswordHash = _authService.HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created succesfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (dbUser == null || !_authService.VerifyPassword(loginUser.PasswordHash, dbUser.PasswordHash))
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı!");
            }

            var token = _authService.GenerateToken(dbUser, _configuration);
            return Ok(new { Token = token });
        }
    }
}
