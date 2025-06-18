using Dto;
using Microsoft.Extensions.Logging;
using Moq;
using MyShopWebApi.Controllers;
using Services;
using Xunit;

namespace TestProject
{
    public class UserControllerUnitTesting
    {
        [Fact]
        public async Task Login_HappyPath_LogsInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UsersController>>();
            var userServiceMock = new Mock<IUserService>();
            var userDto = new UserDTO(1, "testuser", "first", "last");
            userServiceMock.Setup(s => s.Login(It.IsAny<UserLoginDTO>())).ReturnsAsync(userDto);

            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Login(new UserLoginDTO("testuser", "pass"));

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("login testuser")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Login_UnhappyPath_DoesNotLogInformation()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UsersController>>();
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(s => s.Login(It.IsAny<UserLoginDTO>())).ReturnsAsync((UserDTO)null);

            var controller = new UsersController(userServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.Login(new UserLoginDTO("testuser", "wrongpass"));

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }
    }
}