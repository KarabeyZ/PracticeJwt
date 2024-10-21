using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticeJwt.Data;
using PracticeJwt.Dtos;
using PracticeJwt.Models;

namespace PracticeJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtDbContext _jwtDbContext;
        private readonly IConfiguration _configuration;

        public AuthController(JwtDbContext jwtDbContext, IConfiguration configuration)
        {
            _jwtDbContext = jwtDbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Email = registerDto.Email, Password = registerDto.Password };
                _jwtDbContext.Users.Add(user);
                await _jwtDbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);


            }
            return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var userQuery = _jwtDbContext.Users.Where(u => u.Email == loginDto.Email);
                var user = await userQuery.FirstOrDefaultAsync();

                if (user == null)
                {
                    return BadRequest();
                }

                //jwt
                var token = Helper.CreateJwtToken(user.Email, _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);
                return Ok(token);
            }
            return BadRequest();
        }
    }
}
