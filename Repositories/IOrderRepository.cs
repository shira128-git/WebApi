using Entities;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<Order> Create(Order order);
    }
}