
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.UpdateOrder
{
    public record UpdateOrderCommand(int Id, OrderRequest Request) : IRequest<ApiResponse<bool>>;
}
