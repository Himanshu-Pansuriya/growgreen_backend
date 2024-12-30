using growgreen_backend.Data;
using growgreen_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace growgreen_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

    #region UserConstructor
    public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    #endregion

    #region GetUserByID
    [HttpGet("{id}")]
        public IActionResult GetUserByID(int id)
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                var user = users.FirstOrDefault(u => u.UserID == id);

                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    #endregion

    #region InsertUser
    [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("User object cannot be null.");

            try
            {
                bool isInserted = _userRepository.Insert(user);

                if (isInserted)
                    return CreatedAtAction(nameof(GetUserByID), new { id = user.UserID }, user);

                return StatusCode(500, "An error occurred while creating the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    #endregion

    #region UpdateUser
    [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel user)
        {
            if (user == null || user.UserID != id)
                return BadRequest("Invalid user data.");

            try
            {
                bool isUpdated = _userRepository.Update(user); ;

                if (isUpdated)
                    return Ok(new { Message = "User updated successfully!" });

                return StatusCode(500, "An error occurred while updating the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                    return Ok(new { Message = "User deleted successfully!" });

                return StatusCode(500, "An error occurred while deleting the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    #endregion

    #region UserAuthentication
    [HttpGet("auth")]
    public IActionResult UserAuth(String email,String password)
    {
        try
        {
            var user = _userRepository.UserAuth(email,password);

            if (user == null)
                return NotFound($"Wrong email or password.");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion

    }
}