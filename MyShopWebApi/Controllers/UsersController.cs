using Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Services;
using Dto;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //UserService userService = new UserService();

        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }



        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserRegisterDTO user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Trim().Length < 3)
                return BadRequest(new { message = "User name must be at least 3 characters long." });

            if (!string.IsNullOrWhiteSpace(user.FirstName) &&
                !System.Text.RegularExpressions.Regex.IsMatch(user.FirstName, @"^[a-zA-Zא-ת]+$"))
                return BadRequest(new { message = "First name must not contain special characters." });

            if (!string.IsNullOrWhiteSpace(user.LastName) &&
                !System.Text.RegularExpressions.Regex.IsMatch(user.LastName, @"^[a-zA-Zא-ת]+$"))
                return BadRequest(new { message = "Last name must not contain special characters." });

            try
            {
                UserDTO u = await _userService.Register(user);
                if (u != null)
                {
                    return Ok(u);
                }
                return BadRequest(new { message = "Registration failed. Please try again." });
                //use short syntax for returning ActionResult
                //return u != null ? Ok(u) : BadRequest("Registration failed. Please try again.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO user)
        {
            UserDTO u = await _userService.Login(user);
            if (u!=null)
            {
                _logger.LogInformation("login " +  u.UserName);
                return Ok(u);
            }
            return StatusCode(400,"try again");
        }

        [HttpPost("checkPassword")]
        public ActionResult<int> CheckPassword([FromBody] string password)
        {


            int result = _userService.CheckPassword(password);
        //use short syntax
            if (result>-1)
            {
                
                return Ok(result);
            }
            return StatusCode(400, "try again");

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Trim().Length < 3)
                return BadRequest(new { message = "User name must be at least 3 characters long." });

            if (!string.IsNullOrWhiteSpace(user.FirstName) &&
                !System.Text.RegularExpressions.Regex.IsMatch(user.FirstName, @"^[a-zA-Zא-ת]+$"))
                return BadRequest(new { message = "First name must not contain special characters." });

            if (!string.IsNullOrWhiteSpace(user.LastName) &&
                !System.Text.RegularExpressions.Regex.IsMatch(user.LastName, @"^[a-zA-Zא-ת]+$"))
                return BadRequest(new { message = "Last name must not contain special characters." });

            try
            {
                await _userService.UpDate(user, id);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       

    }
}
