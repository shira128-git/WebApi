using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{ 
    public record ProductDTO(int Id, int CategoryId, string ProductName, string ProductDescription, double Price, string ImagePath);
}
