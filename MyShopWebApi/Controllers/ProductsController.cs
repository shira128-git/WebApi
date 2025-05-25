using Microsoft.AspNetCore.Mvc;
using Entities;
using Services;
using Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<List<ProductDTO>> Get(
        [FromQuery] string? desc,
        [FromQuery] int? minPrice,
        [FromQuery] int? maxPrice,
        [FromQuery] int?[] categoryIds)
        {
            return await _productService.Get(desc, minPrice, maxPrice, categoryIds);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
