using Dto;
using Entities;

namespace Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> Get();
    }
}