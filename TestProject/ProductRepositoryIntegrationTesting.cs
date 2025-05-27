using Dto;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task GetProducts_WithValidFilters_ReturnsProducts()
        {
            var 
            // Arrange
            var product1 = new Product { ProductName = "Product1", ProductDescription = "Description1", Price = 10.0, CategoryId = 1 };
            var product2 = new Product { ProductName = "Product2", ProductDescription = "Description2", Price = 20.0, CategoryId = 2 };
            await _shopContext.Products.AddRangeAsync(product1, product2);
            await _shopContext.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetProducts(null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetProducts_WithNoMatchingFilters_ReturnsEmptyList()
        {
            // Act
            var result = await _productRepository.GetProducts("NonExistent", 100, 200, new int[] { });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RegisterProduct_ValidProduct_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { ProductName = "NewProduct", ProductDescription = "New Description", Price = 15.0, CategoryId = 1 };

            // Act
            await _productRepository.AddProduct(product);
            await _shopContext.SaveChangesAsync();

            // Assert
            var addedProduct = await _shopContext.Products.FirstOrDefaultAsync(p => p.ProductName == "NewProduct");
            Assert.NotNull(addedProduct);
            Assert.Equal(product.ProductName, addedProduct.ProductName);
        }

        [Fact]
        public async Task UpdateProduct_ExistingProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { ProductName = "ProductToUpdate", ProductDescription = "Old Description", Price = 10.0, CategoryId = 1 };
            await _shopContext.Products.AddAsync(product);
            await _shopContext.SaveChangesAsync();

            // Act - Update the fields of the existing product
            product.ProductName = "UpdatedProduct";
            product.Price = 20.0;
            await _productRepository.UpdateProduct(product, product.Id);
            await _shopContext.SaveChangesAsync();

            // Assert
            var fetchedProduct = await _shopContext.Products.FindAsync(product.Id);
            Assert.NotNull(fetchedProduct);
            Assert.Equal("UpdatedProduct", fetchedProduct.ProductName);
            Assert.Equal(20.0, fetchedProduct.Price);
        }

        [Fact]
        public async Task DeleteProduct_ExistingProduct_ShouldRemoveProduct()
        {
            // Arrange
            var product = new Product { ProductName = "ProductToDelete", ProductDescription = "Delete Description", Price = 10.0, CategoryId = 1 };
            await _shopContext.Products.AddAsync(product);
            await _shopContext.SaveChangesAsync();

            // Act
            await _productRepository.DeleteProduct(product.Id);
            await _shopContext.SaveChangesAsync();

            // Assert
            var deletedProduct = await _shopContext.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }
    }
}
