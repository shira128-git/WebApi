using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {

        ShopContext DBcontex;
        public ProductRepository(ShopContext _DBcontex)
        {
            DBcontex = _DBcontex;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await DBcontex.Products.ToListAsync();
        }
    }
}
