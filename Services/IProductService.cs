using Entities;
using Dto;
namespace Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> Get(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds);
    }
}