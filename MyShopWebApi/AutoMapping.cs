using AutoMapper;
using Entities;
using Dto;


namespace MyShopWebApi
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
        }
    }
}
