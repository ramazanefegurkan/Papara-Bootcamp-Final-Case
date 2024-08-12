using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Base.Exception
{
    public class CustomException : System.Exception
    {
        public int StatusCode { get; }
        public CustomException(
            string message,
            int statusCode = StatusCodes.Status400BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
