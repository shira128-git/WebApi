using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        ShopContext DBcontex;
        public OrderRepository(ShopContext _DBcontex)
        {
            DBcontex = _DBcontex;
        }
        public async Task<Order> Create(Order order)
        {

            await DBcontex.Orders.AddAsync(order);
            await DBcontex.SaveChangesAsync();
            return await Task.FromResult(order);

        }

    }
}
