using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        ShopContext DBcontex;//_dBcontex - change in all the repositories
        public CategoryRepository(ShopContext _DBcontex)
        {
            DBcontex = _DBcontex;//_dBcontex = dBcontex
        }

        public async Task<List<Category>> GetCategories()
        {
            return await DBcontex.Categories.Include(c=>c.Products).ToListAsync();
        }
    }
}
