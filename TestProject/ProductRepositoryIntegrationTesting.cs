using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class ProductRepositoryIntegrationTesting : IClassFixture<DatabaseFixture>
    {
        private readonly ShopContext _shopContext;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryIntegrationTesting(DatabaseFixture databaseFixture)
        {
            _shopContext = databaseFixture.Context;
            _productRepository = new ProductRepository(_shopContext);
        }

        // בדיקה: שליפת כל המוצרים ללא פילטרים (API Pass)
        [Fact]
        public async Task GetProducts_NoFilters_ReturnsAllProducts()
        {
            // Arrange
            var category = new Category { CategoryName = "Cat1" };
            await _shopContext.Categories.AddAsync(category);
            await _shopContext.SaveChangesAsync();

            var product1 = new Product { ProductName = "Product1", ProductDescription = "Desc1", Price = 10, CategoryId = category.Id };
            var product2 = new Product { ProductName = "Product2", ProductDescription = "Desc2", Price = 20, CategoryId = category.Id };
            await _shopContext.Products.AddRangeAsync(product1, product2);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts(null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        // בדיקה: שליפת מוצרים לפי תיאור (API Pass)
        [Fact]
        public async Task GetProducts_FilterByDescription_ReturnsMatchingProducts()
        {
            // Arrange
            var category = new Category { CategoryName = "Cat2" };
            await _shopContext.Categories.AddAsync(category);
            await _shopContext.SaveChangesAsync();

            var product1 = new Product { ProductName = "Apple", ProductDescription = "Red fruit", Price = 5, CategoryId = category.Id };
            var product2 = new Product { ProductName = "Banana", ProductDescription = "Yellow fruit", Price = 7, CategoryId = category.Id };
            await _shopContext.Products.AddRangeAsync(product1, product2);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts("Red", null, null, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Apple", result[0].ProductName);
        }

        // בדיקה: שליפת מוצרים לפי טווח מחירים (API Pass)
        [Fact]
        public async Task GetProducts_FilterByPriceRange_ReturnsMatchingProducts()
        {
            // Arrange
            var category = new Category { CategoryName = "Cat3" };
            await _shopContext.Categories.AddAsync(category);
            await _shopContext.SaveChangesAsync();

            var product1 = new Product { ProductName = "Cheap", ProductDescription = "Low", Price = 5, CategoryId = category.Id };
            var product2 = new Product { ProductName = "Expensive", ProductDescription = "High", Price = 100, CategoryId = category.Id };
            await _shopContext.Products.AddRangeAsync(product1, product2);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts(null, 10, 200, null);

            // Assert
            Assert.Single(result);
            Assert.Equal("Expensive", result[0].ProductName);
        }

        // בדיקה: שליפת מוצרים לפי קטגוריה (API Pass)
        [Fact]
        public async Task GetProducts_FilterByCategory_ReturnsMatchingProducts()
        {
            // Arrange
            var category1 = new Category { CategoryName = "CatA" };
            var category2 = new Category { CategoryName = "CatB" };
            await _shopContext.Categories.AddRangeAsync(category1, category2);
            await _shopContext.SaveChangesAsync();

            var product1 = new Product { ProductName = "A", ProductDescription = "Desc", Price = 10, CategoryId = category1.Id };
            var product2 = new Product { ProductName = "B", ProductDescription = "Desc", Price = 20, CategoryId = category2.Id };
            await _shopContext.Products.AddRangeAsync(product1, product2);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts(null, null, null, new int?[] { category2.Id });

            // Assert
            Assert.Single(result);
            Assert.Equal("B", result[0].ProductName);
        }

        // בדיקה: שליפת מוצרים ללא התאמה לפילטרים (API Fail)
        [Fact]
        public async Task GetProducts_NoMatchingFilters_ReturnsEmptyList()
        {
            // Arrange
            var category = new Category { CategoryName = "CatX" };
            await _shopContext.Categories.AddAsync(category);
            await _shopContext.SaveChangesAsync();

            var product = new Product { ProductName = "X", ProductDescription = "Desc", Price = 10, CategoryId = category.Id };
            await _shopContext.Products.AddAsync(product);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts("NotExist", 100, 200, new int?[] { 999 });

            // Assert
            Assert.Empty(result);
        }


       
    }
}