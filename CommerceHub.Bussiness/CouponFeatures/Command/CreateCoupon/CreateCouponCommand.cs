
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CouponFeatures.Command.CreateCoupon
{
    public record CreateCouponCommand(CouponRequest Request) : IRequest<ApiResponse<int>>;
}
