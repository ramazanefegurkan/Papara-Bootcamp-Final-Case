using CommerceHub.Base.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class InsufficientStockException : CustomException
    {
        public InsufficientStockException(string message) : base(message, StatusCodes.Status400BadRequest) { }
    }
}
