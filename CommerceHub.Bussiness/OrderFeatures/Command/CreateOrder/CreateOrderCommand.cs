
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder
{
    public record CreateOrderCommand(OrderRequest Request) : IRequest<ApiResponse<OrderResponse>>;
}
