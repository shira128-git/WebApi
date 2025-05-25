using Entities;
using Repositories;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductDTO>> Get(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
        {

            List<Product> Products = await _productRepository.GetProducts(desc,minPrice,maxPrice,categoryIds);
            List<ProductDTO> productDTOs = _mapper.Map<List<Product>, List<ProductDTO>>(Products);
            return productDTOs;
        }
    }
}
