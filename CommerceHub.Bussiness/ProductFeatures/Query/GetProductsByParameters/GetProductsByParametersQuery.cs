using CommerceHub.Data.Enums;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByParameters
{
    public record GetProductsByParametersQuery(string? Name, ProductStatus? Status, decimal? MinPrice, decimal? MaxPrice, string? SortBy, string? SortOrder)
        : IRequest<ApiResponse<IEnumerable<ProductDetailResponse>>>;
}
