using CommerceHub.Base.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException(string message) : base(message,StatusCodes.Status401Unauthorized) { }
    }

}
