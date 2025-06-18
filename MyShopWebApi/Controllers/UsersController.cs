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

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserRegisterDTO user)
        {

            UserDTO u = await _userService.Register(user);
            if (u!=null)
            {
                return Ok(u);
            }
            return StatusCode(400,"try Again");


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
        
            if (result>-1)
            {
                
                return Ok(result);
            }
            return StatusCode(400, "try again");

        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User u)
        {
            try
            {
                await _userService.UpDate(u, id);
                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public void Put(int id, [FromBody]User u)
        //{
        //    //User newUser=new User();

        //    //if(u.firstName!=null)
        //    //{
        //    //    newUser.firstName = u.firstName;
        //    //}
        //    //if (u.lastName != null)
        //    //{
        //    //    newUser.lastName = u.lastName;
        //    //}
        //    //if (u.password != null)
        //    //{
        //    //    newUser.password = u.password;
        //    //}
        //    //if (u.userName != null)
        //    //{
        //    //    newUser.userName = u.userName;
        //    //}
        //    //newUser.userId = id;
        //    //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "users.txt");


        //    //string textToReplace = string.Empty;
        //    //using (StreamReader reader = System.IO.File.OpenText(filePath))
        //    //{
        //    //    string currentUserInFile;
        //    //    while ((currentUserInFile = reader.ReadLine()) != null)
        //    //    {

        //    //        User user = JsonSerializer.Deserialize<User>(currentUserInFile);
        //    //        if (user.userId == id)
        //    //            textToReplace = currentUserInFile;
        //    //    }
        //    //}

        //    //if (textToReplace != string.Empty)
        //    //{
        //    //    string text = System.IO.File.ReadAllText(filePath);
        //    //    text = text.Replace(textToReplace, JsonSerializer.Serialize(newUser));
        //    //    System.IO.File.WriteAllText(filePath, text);
        //    //}

        //    _userService.UpDate(u,id);

        //}

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
