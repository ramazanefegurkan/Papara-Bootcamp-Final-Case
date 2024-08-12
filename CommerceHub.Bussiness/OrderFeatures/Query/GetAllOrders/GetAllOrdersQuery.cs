
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Domain;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetAllOrders
{
	public record GetAllOrdersQuery : IRequest<ApiResponse<IEnumerable<OrderResponse>>>;
}
