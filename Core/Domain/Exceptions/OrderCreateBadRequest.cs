using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
    {
    public class OrderCreateBadRequest() : BadRequestException("Invalid Operation when creating or updating the Order!")
        {
        }
    }
