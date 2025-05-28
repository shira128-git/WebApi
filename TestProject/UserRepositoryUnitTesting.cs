using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Xunit;

namespace TestProject
{
    public class UserRepositoryUnitTesting
    {
        // בדיקה: התחברות עם משתמש קיים (Pass)
        [Fact]
        public async Task Login_UserExists_ReturnUser()
        {
            // Arrange
            var user = new User { UserName = "test", Password = "test" };
            var mockContext = new Mock<ShopContext>();
            var users = new List<User>() { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Login(user.UserName);

            // Assert
            Assert.Equal(user, result);
        }

        // בדיקה: התחברות עם משתמש לא קיים (Fail)
        [Fact]
        public async Task Login_UserNotExists_ReturnNull()
        {
            // Arrange
            var mockContext = new Mock<ShopContext>();
            var users = new List<User>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Login("testName");

            // Assert
            Assert.Null(result);
        }

        // בדיקה: התחברות עם שם משתמש null (Fail)
        [Fact]
        public async Task Login_NullUserName_ReturnNull()
        {
            // Arrange
            var mockContext = new Mock<ShopContext>();
            var users = new List<User>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Login(null);

            // Assert
            Assert.Null(result);
        }

        // בדיקה: רישום משתמש חדש (Pass)
        [Fact]
        public async Task Register_NewUser_ReturnUser()
        {
            // Arrange
            var user = new User { UserName = "newUser", Password = "password" };
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.Register(user);

            // Assert
            Assert.Equal(user, result);
            mockContext.Verify(x => x.Users.AddAsync(user, default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        // בדיקה: רישום משתמש עם שם קיים (Fail)
        [Fact]
        public async Task Register_DuplicateUserName_ShouldNotAddDuplicate()
        {
            // Arrange
            var user = new User { UserName = "duplicateUser", Password = "password" };
            var users = new List<User> { user };
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            await userRepository.Register(user);

            // Assert
            // ודא שיש רק משתמש אחד עם אותו שם
            Assert.Equal(1, users.Count(u => u.UserName == "duplicateUser"));
        }

        // בדיקה: רישום משתמש עם שדות ריקים/לא תקינים (Fail)
        [Fact]
        public async Task Register_InvalidUser_ShouldNotAdd()
        {
            // Arrange
            var user = new User { UserName = null, Password = null, FirstName = null, LastName = null };
            var users = new List<User>();
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            await userRepository.Register(user);

            // Assert
            Assert.DoesNotContain(users, u => u.UserName == null);
        }

        // בדיקה: קבלת כל המשתמשים (Pass)
        [Fact]
        public void GetUsers_ReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserName = "user1", Password = "password1" },
                new User { UserName = "user2", Password = "password2" }
            };
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetUsers();

            // Assert
            Assert.Equal(users, result);
        }

        // בדיקה: עדכון משתמש קיים (Pass)
        [Fact]
        public async Task UpDate_ExistingUser_ReturnUpdatedUser()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "existingUser", Password = "oldPassword" };
            var updatedUser = new User { Id = 1, UserName = "existingUser", Password = "newPassword" };
            var mockContext = new Mock<ShopContext>();
            var users = new List<User> { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.UpDate(updatedUser, user.Id);

            // Assert
            Assert.Equal(updatedUser, result);
            mockContext.Verify(x => x.Users.Update(updatedUser), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        // בדיקה: עדכון משתמש שלא קיים (Fail)
        [Fact]
        public async Task UpDate_NonExistingUser_ShouldNotUpdate()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "notExists", Password = "password" };
            var users = new List<User>(); // רשימה ריקה = אין משתמשים קיימים
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);
            var userRepository = new UserRepository(mockContext.Object);

            // Act
            await userRepository.UpDate(user, user.Id);

            // Assert
            // ודא שהמשתמש לא נוסף לרשימה
            Assert.DoesNotContain(users, u => u.Id == user.Id);
        }
    }
}