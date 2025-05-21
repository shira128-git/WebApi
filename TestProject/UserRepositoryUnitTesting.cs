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
        [Fact]
        public async Task Login_UserExists_ReturnUser()
        {
            var user = new User { UserName = "test", Password = "test" };

            var mockContext = new Mock<ShopContext>();
            var users = new List<User>() { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var result = await userRepository.Login(user.UserName);

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task Login_NotUserExists_ReturnNull()
        {
            var mockContext = new Mock<ShopContext>();
            var users = new List<User>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var result = await userRepository.Login("testName");

            Assert.Null(result);
        }

        [Fact]
        public async Task Register_NewUser_ReturnUser()
        {
            var user = new User { UserName = "newUser", Password = "password" };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(new List<User>());

            var userRepository = new UserRepository(mockContext.Object);

            var result = await userRepository.Register(user);

            Assert.Equal(user, result);
            mockContext.Verify(x => x.Users.AddAsync(user, default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public void GetUsers_ReturnAllUsers()
        {
            var users = new List<User>
            {
                new User { UserName = "user1", Password = "password1" },
                new User { UserName = "user2", Password = "password2" }
            };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var result = userRepository.GetUsers();

            Assert.Equal(users, result);
        }

        [Fact]
        public async Task UpDate_ExistingUser_ReturnUpdatedUser()
        {
            var user = new User { UserName = "existingUser", Password = "oldPassword" };
            var updatedUser = new User { UserName = "existingUser", Password = "newPassword" };

            var mockContext = new Mock<ShopContext>();
            var users = new List<User> { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var result = await userRepository.UpDate(updatedUser, user.Id);

            Assert.Equal(updatedUser, result);
            mockContext.Verify(x => x.Users.Update(updatedUser), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}