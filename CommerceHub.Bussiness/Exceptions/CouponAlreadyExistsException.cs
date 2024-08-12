using CommerceHub.Base.Exception;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class CouponAlreadyExistsException : CustomException
    {
        public CouponAlreadyExistsException() : base("A coupon with this code already exists.", StatusCodes.Status400BadRequest) { }
    }
}
