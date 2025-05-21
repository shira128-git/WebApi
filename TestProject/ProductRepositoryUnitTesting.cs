using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class ProductRepositoryUnitTesting
    {
        [Fact]
        public async Task GetProducts_ShouldReturnFilteredProducts()
        {
            // Arrange
            var category1 = new Category { Id = 1, CategoryName = "Electronics" };
            var category2 = new Category { Id = 2, CategoryName = "Books" };

            var products = new List<Product>
            {
                new Product { Id = 1, ProductName = "Laptop", ProductDescription = "Gaming Laptop", Price = 1500, CategoryId = 1, Category = category1 },
                new Product { Id = 2, ProductName = "Book", ProductDescription = "Science Book", Price = 50, CategoryId = 2, Category = category2 },
                new Product { Id = 3, ProductName = "Phone", ProductDescription = "Smartphone", Price = 800, CategoryId = 1, Category = category1 }
            };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProducts("Laptop", 1000, 2000, new int?[] { 1 });

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Laptop", result.First().ProductName);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnAllProducts_WhenNoFiltersApplied()
        {
            // Arrange
            var category = new Category { Id = 1, CategoryName = "General" };

            var products = new List<Product>
            {
                new Product { Id = 1, ProductName = "Item1", ProductDescription = "Description1", Price = 100, CategoryId = 1, Category = category },
                new Product { Id = 2, ProductName = "Item2", ProductDescription = "Description2", Price = 200, CategoryId = 1, Category = category }
            };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProducts(null, null, null, Array.Empty<int?>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnEmpty_WhenNoMatchingProducts()
        {
            // Arrange
            var category = new Category { Id = 1, CategoryName = "General" };

            var products = new List<Product>
            {
                new Product { Id = 1, ProductName = "Item1", ProductDescription = "Description1", Price = 100, CategoryId = 1, Category = category }
            };

            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProducts("NonExistent", 500, 1000, new int?[] { 2 });

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}