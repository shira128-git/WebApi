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
        ShopContext DBcontex;
        public CategoryRepository(ShopContext _DBcontex)
        {
            DBcontex = _DBcontex;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await DBcontex.Categories.Include(c=>c.Products).ToListAsync();
        }
    }
}
