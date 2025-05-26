using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public record UserRegisterDTO(string UserName, string Password, string FirstName, string LastName);
    
}
