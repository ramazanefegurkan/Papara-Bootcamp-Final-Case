using CommerceHub.Data.Enums;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetOrdersByParameters
{
    public record GetOrdersByParametersQuery(string? OrderNumber, decimal? MinAmount, decimal? MaxAmount, string? SortBy, string? SortOrder)
        : IRequest<ApiResponse<IEnumerable<OrderResponse>>>;
}

