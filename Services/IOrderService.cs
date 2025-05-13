using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<Order> Create(Order order);
    }
}