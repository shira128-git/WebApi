using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // GET: api/<CategoriesController>
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<List<CategoryDTO>> Get()
        {
            return await _categoryService.Get();
        }

     
    }
}
