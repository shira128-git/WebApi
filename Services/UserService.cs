using AutoMapper;
using Dto;
using Entities;

using Repositories;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> Register(UserRegisterDTO user)
        {
            if (CheckPassword(user.Password) < 2)
                throw new Exception("Password is too weak.");
            //יתכן שמומלץ לעשות גם כאן ולידציות אך נמנענו מכך כדי להמנע מטסטים נוספים//delete
            //if (user.UserName == null || user.UserName.Trim().Length < 3)
            //    throw new Exception("User name must be at least 3 characters long.");

            //if ((!string.IsNullOrWhiteSpace(user.FirstName) &&
            //     !System.Text.RegularExpressions.Regex.IsMatch(user.FirstName, @"^[a-zA-Zא-ת]+$")) ||
            //    (!string.IsNullOrWhiteSpace(user.LastName) &&
            //     !System.Text.RegularExpressions.Regex.IsMatch(user.LastName, @"^[a-zA-Zא-ת]+$")))
            //    throw new Exception("First name and last name must not contain special characters.");

            List<User> users = _userRepository.GetUsers();
            User userfound = users.FirstOrDefault(u => u.UserName.Trim() == user.UserName);
            if (userfound != null)
                throw new Exception("User name already exists");

            var newUser = new User()
            {
                FirstName = user.FirstName?.Trim(),
                LastName = user.LastName?.Trim(),
                UserName = user.UserName.Trim(),
                Password = user.Password.Trim(),
            };

            return _mapper.Map<UserDTO>(await _userRepository.Register(newUser));
        }

        public async Task<UserDTO> Login(UserLoginDTO user)
        {
            User userfound = await _userRepository.Login(user.UserName);
          
            // if (userfound == null)
            // {
            //     return null;
            // }
            if (userfound?.Password.Trim() == user.Password)
            {
                return  _mapper.Map<UserDTO>(userfound);
            }
           
            return null;
        }

        public async Task<User> UpDate(User user, int id)
        {
         
            List<User> users = _userRepository.GetUsers();
            User existingUser = users.FirstOrDefault(u => u.UserName.Trim() == user.UserName.Trim() && u.Id != id);
            if (existingUser != null)
            {
                throw new Exception("User name already exists");
            }
            if (CheckPassword(user.Password) < 2)
            {
                throw new Exception("Password is low");
            }
            //יתכן שמומלץ לעשות גם כאן ולידציות אך נמנענו מכך כדי להמנע מטסטים נוספים
            //if (user.UserName == null || user.UserName.Trim().Length < 3)
            //    throw new Exception("User name must be at least 3 characters long.");

            //if (!System.Text.RegularExpressions.Regex.IsMatch(user.FirstName, @"^[a-zA-Zא-ת]+$") ||
            //    !System.Text.RegularExpressions.Regex.IsMatch(user.LastName, @"^[a-zA-Zא-ת]+$"))
            //    throw new Exception("First name and last name must not contain special characters.");

            return await _userRepository.UpDate(user, id);
        }

        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;

        }
    }
}
