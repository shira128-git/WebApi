using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class CategoryRepositoryIntegrationTesting : IClassFixture<DatabaseFixture>
    {
        private readonly ShopContext _shopContext;
        private readonly CategoryRepository _categoryRepository;

        public CategoryRepositoryIntegrationTesting(DatabaseFixture databaseFixture)
        {
            _shopContext = databaseFixture.Context;
            _categoryRepository = new CategoryRepository(_shopContext);
        }

        // בדיקה: שליפת כל הקטגוריות כאשר קיימות קטגוריות (API Pass)
        [Fact]
        public async Task GetCategories_WithCategories_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { CategoryName = "Clothing" },
                new Category { CategoryName = "Toys" }
            };
            await _shopContext.Categories.AddRangeAsync(categories);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
            Assert.Contains(result, c => c.CategoryName == "Clothing");
            Assert.Contains(result, c => c.CategoryName == "Toys");
        }

        // בדיקה: שליפת כל הקטגוריות כאשר אין קטגוריות (API Pass)
        [Fact]
        public async Task GetCategories_NoCategories_ReturnsEmptyList()
        {
            // Arrange
            foreach (var category in _shopContext.Categories)
                _shopContext.Categories.Remove(category);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _categoryRepository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

    }
}
