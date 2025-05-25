using AutoMapper;
using Dto;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<OrderDTO> Create(OrderDTO order)
        {
            var order1 = _mapper.Map<Order>(order);
            var o = await _orderRepository.Create(order1);
            return _mapper.Map<OrderDTO>(o);

        }
    }
}
