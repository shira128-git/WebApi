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

        [Fact]
        public async Task GetUser_ValidCredentials_ReturnUser()
        {
            //Arrange
            var user = new User{ UserName = "test", Password = "Ss!123bb", FirstName="test", LastName="test" };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();
            // Act
            var result = await _userRepository.Login(user.UserName);
            // Assert
            Assert.NotNull(result);

        }

        //בדיקת רישום משתמש חדש
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
        //בדיקת התחברות עם משתמש קיים
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
        //בדיקת התחברות עם משתמש לא קיים

        [Fact]
        public async Task Login_NotExistingUser_ShouldReturnNull()

        {

            // Act
            var result = await _userRepository.Login("nonExistingUser");

            // Assert
            Assert.Null(result);

        }

        //בדיקת עדכון משתמש
        [Fact]
        public async Task UpDate_ExistingUser_ShouldUpdateUser()
        {
            // Arrange
            var user = new User { UserName = "userToUpdate", Password = "Password1!", FirstName = "User", LastName = "ToUpdate" };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();

            // Act - עדכון השדות של המשתמש הקיים
            user.FirstName = "Updated"; // עדכון השם
            user.Password = "Password2!"; // עדכון הסיסמא
            await _userRepository.UpDate(user, user.Id);

            // Assert
            var fetchedUser = await _shopContext.Users.FindAsync(user.Id);
            Assert.NotNull(fetchedUser);
            Assert.Equal("Updated", fetchedUser.FirstName);
            Assert.Equal("Password2!", fetchedUser.Password); // ודא שהסיסמא גם עודכנה
        }

     }
}
