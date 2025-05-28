using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class OrderRepositoryIntegrationTesting : IClassFixture<DatabaseFixture>
    {
        private readonly ShopContext _shopContext;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryIntegrationTesting(DatabaseFixture databaseFixture)
        {
            _shopContext = databaseFixture.Context;
            _orderRepository = new OrderRepository(_shopContext);
        }

        // בדיקה: יצירת הזמנה תקינה (API Pass)
        [Fact]
        public async Task Create_ValidOrder_ShouldAddOrder()
        {
            // Arrange
            var user = new User
            {
                UserName = "orderuser",
                Password = "pass123",
                FirstName = "Order",
                LastName = "User"
            };
            await _shopContext.Users.AddAsync(user);
            await _shopContext.SaveChangesAsync();

            var order = new Order
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 100,
                UserId = user.Id
            };

            // Act
            var result = await _orderRepository.Create(order);

            // Assert
            Assert.NotNull(result);
            var addedOrder = await _shopContext.Orders.FirstOrDefaultAsync(o => o.Id == result.Id);
            Assert.NotNull(addedOrder);
            Assert.Equal(100, addedOrder.OrderSum);
            Assert.Equal(user.Id, addedOrder.UserId);
        }

        // בדיקה: יצירת הזמנה עם שדות ריקים/לא תקינים (API Fail)
        [Fact]
        public async Task Create_InvalidOrder_ShouldFail()
        {
            // Arrange
            var order = new Order
            {
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _orderRepository.Create(order);
            });
        }
    }
}
