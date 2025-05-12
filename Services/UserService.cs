using Entities;

using Repositories;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<User> Login(string UserName, string Password)
        {

            User userfound = await _userRepository.Login(UserName);
            if (userfound == null)
            {
                return null;
            }
            if (userfound.Password.Trim() == Password)
            {
                return userfound;
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
