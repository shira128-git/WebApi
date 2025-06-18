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
        public async Task<UserDTO> Register(UserRegisterDTO  user)
        {
            //return userRepository.Register(user);
            if (CheckPassword(user.Password) < 2)
            {
                return null;
            }
            List<User> users = _userRepository.GetUsers();
            User userfound =users.FirstOrDefault(u => u.UserName.Trim() == user.UserName);
            if (userfound == null)
            {

                var newUser = new User()
                {
                    FirstName=user.FirstName.Trim(),
                    LastName = user.LastName.Trim(),
                    UserName = user.UserName.Trim(),
                    Password = user.Password.Trim(),
                };

                return  _mapper.Map<UserDTO>(await _userRepository.Register(newUser));
            }
            return null;
        }

        public async Task<UserDTO> Login(UserLoginDTO user)
        {
            User userfound = await _userRepository.Login(user.UserName);
          
            if (userfound == null)
            {
                return null;
            }
            if (userfound.Password.Trim() == user.Password)
            {
                return  _mapper.Map<UserDTO>(userfound);
            }
           
            return null;
        }


        //public async Task<User> UpDate(User user, int id)
        //{
        //    return await _userRepository.UpDate(user, id);
        //}
        public async Task<User> UpDate(User user, int id)
        {
            // בדיקת ייחודיות שם משתמש
            List<User> users = _userRepository.GetUsers();
            User existingUser = users.FirstOrDefault(u => u.UserName.Trim() == user.UserName.Trim() && u.Id != id);
            if (existingUser != null)
            {
                throw new Exception("User name already exists");
            }

            return await _userRepository.UpDate(user, id);
        }

        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;

        }
    }
}
