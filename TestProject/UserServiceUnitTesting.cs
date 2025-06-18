using Dto;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Xunit;

namespace TestProject
{
    public class UserServiceUnitTesting
    {
        [Fact]
        public void CheckPassword_StrongPassword_ShouldReturnHighStrength()
        {
            // Arrange
            var password = "StrongPass1"; 

            var expectedStrength = 3;
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.CheckPassword(password)).Returns(expectedStrength);

            // Act
            var result = mockUserService.Object.CheckPassword(password);

            // Assert
            Assert.Equal(expectedStrength, result);
        }

        [Fact]
        public void CheckPassword_WeakPassword_ShouldReturnLowStrength()
        {
            // Arrange
            var password = "weakpass"; // סיסמה חלשה
            var expectedStrength = 1;
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.CheckPassword(password)).Returns(expectedStrength);

            // Act
            var result = mockUserService.Object.CheckPassword(password);

            // Assert
            Assert.Equal(expectedStrength, result);
        }

        
    }
}

