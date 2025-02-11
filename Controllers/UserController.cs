using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        #region UserConstructor
        public UserController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        #endregion

        #region GetAllUser
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                List<UserModel> users = _userRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

        #region GetUserByID
        [HttpGet("{id}")]
        public IActionResult GetUserByID(int id)
        {
            try
            {
                var user = _userRepository.GetAllUsers().FirstOrDefault(u => u.UserID == id);

                if (user == null)
                    return NotFound(new { message = $"User with ID {id} not found." });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

        #region InsertUser
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest(new { message = "User object cannot be null." });

            try
            {
                bool isInserted = _userRepository.Insert(user);

                if (isInserted)
                    return CreatedAtAction(nameof(GetUserByID), new { id = user.UserID }, user);

                return StatusCode(500, new { message = "An error occurred while creating the user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

        #region UpdateUser
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        {
            if (user == null || user.UserID != id)
                return BadRequest(new { message = "Invalid user data." });

            try
            {
                bool isUpdated = _userRepository.Update(user);

                if (isUpdated)
                    return Ok(new { message = "User updated successfully!" });

                return StatusCode(500, new { message = "An error occurred while updating the user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

        #region DeleteUser
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool isDeleted = _userRepository.Delete(id);

                if (isDeleted)
                    return Ok(new { message = "User deleted successfully!" });

                return StatusCode(500, new { message = "An error occurred while deleting the user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }
        #endregion

    }
}
