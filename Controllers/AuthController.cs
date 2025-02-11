using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        #region UserConstructor
        public AuthController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        #endregion

        #region UserAuthentication
        [HttpPost]
        public IActionResult UserAuth([FromBody] UserLoginModel users)
        {
            try
            {
                var user = _userRepository.UserAuth(users.Email, users.Password, users.Role);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid credentials" });
                }

                // Generate JWT token
                var token = GenerateJWTToken(user);

                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        user.UserID,
                        user.UserName,
                        user.Email,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

        #region GenerateJWTToken
        private string GenerateJWTToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
        #endregion

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
