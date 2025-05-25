using Dto;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDTO> Create(OrderDTO order);
    }
}