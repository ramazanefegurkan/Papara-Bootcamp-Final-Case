
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetOrderById
{
    public record GetOrderByIdQuery(int Id) : IRequest<ApiResponse<OrderResponse>>;
}
