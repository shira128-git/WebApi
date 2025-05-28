using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Xunit;

namespace TestProject
{
    public class CategoryRepositoryUnitTesting
    {
        // API Pass: שליפת כל הקטגוריות עם מוצרים
        [Fact]
        public async Task GetCategories_ReturnAllCategoriesWithProducts()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Electronics", Products = new List<Product> { new Product { Id = 1, ProductName = "Laptop" } } },
                new Category { Id = 2, CategoryName = "Books", Products = new List<Product> { new Product { Id = 2, ProductName = "Novel" } } }
            };
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);
            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.CategoryName == "Electronics" && c.Products.Count == 1);
            Assert.Contains(result, c => c.CategoryName == "Books" && c.Products.Count == 1);
        }

        // API Fail: שליפת קטגוריות כאשר אין קטגוריות כלל
        [Fact]
        public async Task GetCategories_NoCategories_ReturnsEmptyList()
        {
            // Arrange
            var categories = new List<Category>();
            var mockContext = new Mock<ShopContext>();
            mockContext.Setup(x => x.Categories).ReturnsDbSet(categories);
            var categoryRepository = new CategoryRepository(mockContext.Object);

            // Act
            var result = await categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}



