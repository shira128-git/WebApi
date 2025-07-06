using Dto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
//In all the controllers - notice:
//1.delete comments 
//2.clean code - change the names of the methods to be more descriptive-  change in all the layers
//3.use short syntax for returning ActionResult

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860//

namespace MyShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }



        // POST api/<OrdersController>//
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO u = await _orderService.Create(order);
            if (u != null)
            {
                return Ok(u);
            }
            return StatusCode(400, "try Again");
            //use short syntax for returning ActionResult
            //return u != null ? Ok(u) : BadRequest("try Again");
        }
    }
}
