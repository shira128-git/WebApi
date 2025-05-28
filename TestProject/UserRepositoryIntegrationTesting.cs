using Dto;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class UserRepositoryIntegrationTesting : IClassFixture<DatabaseFixture>
    {
        private readonly ShopContext _shopContext;
        private readonly UserRepository _userRepository;

        public UserRepositoryIntegrationTesting(DatabaseFixture databaseFixture)
        {
            _shopContext = databaseFixture.Context;
            _userRepository = new UserRepository(_shopContext);
        }

        // בדיקה של התחברות עם פרטי משתמש תקינים
        [Fact]
        public async Task GetUser_ValidCredentials_ReturnUser()
        {
            // Arrange
            var user = new User { UserName = "test", Password = "Ss!123bb", FirstName = "test", LastName = "test" };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login(user.UserName);

            // Assert
            Assert.NotNull(result);
        }

        // בדיקת רישום משתמש חדש
        [Fact]
        public async Task Register_ValidUser_ShouldAddUser()
        {
            // Arrange
            var user = new User { UserName = "newUser", Password = "Password1!", FirstName = "New", LastName = "User" };

            // Act
            var result = await _userRepository.Register(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newUser", result.UserName);
            var addedUser = await _shopContext.Users.FirstOrDefaultAsync(u => u.UserName == "newUser");
            Assert.NotNull(addedUser);
        }

        // בדיקת התחברות עם משתמש קיים
        [Fact]
        public async Task Login_ExistingUser_ShouldReturnUser()
        {
            // Arrange
            var user = new User { UserName = "existingUser", Password = "Password1!", FirstName = "Existing", LastName = "User" };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.Login(user.UserName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("existingUser", result.UserName);
        }

        // בדיקת התחברות עם משתמש לא קיים
        [Fact]
        public async Task Login_NotExistingUser_ShouldReturnNull()
        {
            // Arrange
            var userName = "nonExistingUser";

            // Act
            var result = await _userRepository.Login(userName);

            // Assert
            Assert.Null(result);
        }

        // בדיקת עדכון משתמש קיים
        [Fact]
        public async Task UpDate_ExistingUser_ShouldUpdateUser()
        {
            // Arrange
            var user = new User { UserName = "userToUpdate", Password = "Password1!", FirstName = "User", LastName = "ToUpdate" };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();
            user.FirstName = "Updated";
            user.Password = "Password2!";

            // Act
            await _userRepository.UpDate(user, user.Id);

            // Assert
            var fetchedUser = await _shopContext.Users.FindAsync(user.Id);
            Assert.NotNull(fetchedUser);
            Assert.Equal("Updated", fetchedUser.FirstName);
            Assert.Equal("Password2!", fetchedUser.Password);
        }

        // בדיקה שמוודאת שהפונקציה GetUsers מחזירה את כל המשתמשים הקיימים במסד הנתונים
        [Fact]
        public void GetUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserName = "user1", Password = "pass1", FirstName = "First1", LastName = "Last1" },
                new User { UserName = "user2", Password = "pass2", FirstName = "First2", LastName = "Last2" }
            };
            _shopContext.Users.AddRange(users);
            _shopContext.SaveChanges();

            // Act
            var result = _userRepository.GetUsers();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count >= 2);
            Assert.Contains(result, u => u.UserName == "user1");
            Assert.Contains(result, u => u.UserName == "user2");
        }

       // //   בדיקה שניסיון לרשום משתמש עם שם קיים זורק חריגה
       ////להוסיף בסמינר יוניק לשדה היוזר ניים
       // [Fact]
       // public async Task Register_DuplicateUserName_ShouldFail()
       // {
       //     // Arrange
       //     var user1 = new User { UserName = "duplicateUser", Password = "Password1!", FirstName = "First", LastName = "User" };
       //     var user2 = new User { UserName = "duplicateUser", Password = "Password2!", FirstName = "Second", LastName = "User" };
       //     await _userRepository.Register(user1);

       //     // Act & Assert
       //     await Assert.ThrowsAsync<DbUpdateException>(async () => await _userRepository.Register(user2));
       // }

        // בדיקה שניסיון לעדכן משתמש שלא קיים זורק חריגה
        [Fact]
        public async Task Update_NonExistingUser_ShouldFail()
        {
            // Arrange
            var user = new User { Id = 9999, UserName = "ghost", Password = "none", FirstName = "Ghost", LastName = "User" };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _userRepository.UpDate(user, user.Id));
        }

        // בדיקה שניסיון לרשום משתמש עם שדות ריקים/לא תקינים זורק חריגה
        [Fact]
        public async Task Register_InvalidUser_ShouldFail()
        {
            // Arrange
            var user = new User { UserName = null, Password = null, FirstName = null, LastName = null };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _userRepository.Register(user));
        }

        // בדיקה שניסיון להתחבר עם שם משתמש null מחזיר null
        [Fact]
        public async Task Login_NullUserName_ShouldReturnNull()
        {
            // Arrange
            string userName = null;

            // Act
            var result = await _userRepository.Login(userName);

            // Assert
            Assert.Null(result);
        }
    }
}
