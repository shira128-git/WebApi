using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<Product>> Get(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
        {

            List<Product> Products = await _productRepository.GetProducts(desc,minPrice,maxPrice,categoryIds);
            return Products;
        }
    }
}
