using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Dto
{
    public record OrderDTO(DateOnly OrderDate, double OrderSum, int UserId, List<OrderItemDTO> OrderItems);
}
