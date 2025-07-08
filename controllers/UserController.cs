using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Entites.Users;

namespace ToDoList.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register/{username}/{password}/{type}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Register(string username, string password, UserType type)
        {
            _userService.Register(username, password, type);
            return Ok("user has been added sucessfully");
        }
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("login/{username}/{password}")]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var user = _userService.Login(username, password);
                var token = _jwtService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
        }

        /*[Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPost("update")]
        public IActionResult Update([FromBody] User user)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            long userId = long.Parse(userIdStr);
            try
            {
                _userService.UpdateUser(userId, user);
                return Ok("user updated");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("user logged in is not the actual user and the user is not an admin");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("there is no such user to update");
            }
        }*/
    }
}