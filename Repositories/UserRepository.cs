

using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {

        ShopContext DBcontex;
        public UserRepository(ShopContext _DBcontex)
        {
            DBcontex = _DBcontex;
        }
        public List<User> GetUsers()
        {
            //List<User> users = System.IO.File.Exists("users.txt") ? System.IO.File.ReadLines("users.txt").Select(line => JsonSerializer.Deserialize<User>(line)).ToList() : new List<User>();
            //return users;

            return DBcontex.Users.ToList();
        }


        public async Task<User> Register(User user)
        {
            //List<User> users = GetUsers();//
            //user.userId = users.Any() ? users.Max(u => u.userId) + 1 : 1;//
            //System.IO.File.AppendAllText("users.txt", JsonSerializer.Serialize(user) + Environment.NewLine);//
            //return user;

            await DBcontex.Users.AddAsync(user);
            await DBcontex.SaveChangesAsync();
            // No need to use Task.FromResult here because the method is already async. Just return the user directly.
            // return await Task.FromResult(user);
            return user;
        }



        public async Task<User> Login(string userName)
        {
            User userLog = await DBcontex.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        
            return userLog;
        }

        //public async Task<User> UpDate(User user, int id) - delete 
        //{

        //    DBcontex.Users.Update(user);
        //    await DBcontex.SaveChangesAsync();


        //    return user;

        //}

        public async Task<User> UpDate(User user, int id)
        {
            // var existingUser = await DBcontex.Users.FindAsync(id);
            // if (existingUser == null)
            //     throw new Exception("User not found");

            // // עדכון שדות
            // existingUser.FirstName = user.FirstName;
            // existingUser.LastName = user.LastName;
            // existingUser.UserName = user.UserName;
            // existingUser.Password = user.Password;
            DBcontex.Users.Update(existingUser);
            await DBcontex.SaveChangesAsync();
            return existingUser;
        }



    }
}
