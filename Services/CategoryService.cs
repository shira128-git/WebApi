using AutoMapper;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDTO>> Get()
        {

            List<Category> categories = await _categoryRepository.GetCategories();
            List<CategoryDTO> categoriesDTOs = _mapper.Map<List<Category>, List<CategoryDTO>>(categories);
            return categoriesDTOs;
        }
    }
}
