using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService >();

builder.Services.AddScoped<IProductRepository, ProductRepository >();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddDbContext<ShopContext>(options =>
    options.UseSqlServer("Data Source=SRV2\\PUPILS;Initial Catalog=Shop ; Integrated Security=True; Trusted_Connection=True;TrustServerCertificate=True"));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


//var app = builder.Build();

//// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.MapOpenApi();
////}

builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
