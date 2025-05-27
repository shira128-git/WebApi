using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
   
    public class DatabaseFixture : IDisposable
    {

        public ShopContext Context { get; private set; }
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ShopContext>()
                .UseSqlServer("Server = SRV2\\PUPILS; Database = ShopTest_Integration ; Trusted_Connection = True; TrustServerCertificate = True")
                .Options;
            Context = new ShopContext(options);
            Context.Database.EnsureCreated();

        }
        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

}
