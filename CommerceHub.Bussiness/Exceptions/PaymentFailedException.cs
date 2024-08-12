﻿using CommerceHub.Base.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class PaymentFailedException : CustomException
    {
        public PaymentFailedException(string message) : base(message,StatusCodes.Status400BadRequest) { }
    }

}
