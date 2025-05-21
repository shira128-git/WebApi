using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class OrderRepositoryUnitTesting
    {
        [Fact]
        public async Task Create_ShouldAddOrderToDatabase()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                OrderDate = new DateOnly(2025, 5, 21),
                OrderSum = 100.50,
                UserId = 1
            };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Orders).ReturnsDbSet(new List<Order>());

            var repository = new OrderRepository(mockContext.Object);

            // Act
            var result = await repository.Create(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order, result);
            mockContext.Verify(x => x.Orders.AddAsync(order, default), Times.Once);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}
