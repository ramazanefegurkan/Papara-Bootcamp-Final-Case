using CommerceHub.Base.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
    }
}
