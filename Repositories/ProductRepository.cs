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

        //public async Task<List<Product>> GetProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
        //{


        //    var query = DBcontex.Products
        //        .Include(product => product.Category)
        //        .Where(product =>
        //    (desc == null ? (true) : (product.ProductDescription.Contains(desc)))
        //    && (minPrice == null ? (true) : (product.Price >= minPrice))
        //    && (maxPrice == null ? (true) : (product.Price <= maxPrice))
        //    && ((categoryIds.Length == 0) ? (true) : (categoryIds.Contains(product.CategoryId)))).OrderBy(product => product.Price);

        //    List<Product> products = await query.ToListAsync();
        //    return products;

        //    //return await DBcontex.Products.Include(c=>c.Category).ToListAsync();

        //}

        public async Task<List<Product>> GetProducts(string? desc, int? minPrice, int? maxPrice, int?[] categoryIds)
        {
            categoryIds ??= Array.Empty<int?>();

            var query = DBcontex.Products
                .Include(product => product.Category)
                .Where(product =>
                    (desc == null ? true : product.ProductDescription.Contains(desc)) &&
                    (minPrice == null ? true : product.Price >= minPrice) &&
                    (maxPrice == null ? true : product.Price <= maxPrice) &&
                    ((categoryIds.Length == 0) ? true : categoryIds.Contains(product.CategoryId))
                )
                .OrderBy(product => product.Price);

            return await query.ToListAsync();
        }
    }
}
