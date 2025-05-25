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
        public async Task<User> Register(User user)
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
                return await _userRepository.Register(user);
            }
            return null;
        }

        public async Task<UserLoginDTO> Login(string UserName, string Password)
        {
            var user = _mapper.Map<User>(UserName);
            var res = await _userRepository.Login(UserName);
            if (res == null)
            {
                return null;
            }
            if (res.Password.Trim() == Password)
            {
                return _mapper.Map<UserLoginDTO>(res);
            }
            return null;
        }

        public async Task<User> UpDate(User user, int id)
        {
            return await _userRepository.UpDate(user, id);
        }

        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;

        }
    }
}
