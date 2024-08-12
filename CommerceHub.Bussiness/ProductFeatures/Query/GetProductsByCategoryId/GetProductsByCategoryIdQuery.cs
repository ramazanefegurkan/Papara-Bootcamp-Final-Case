using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByCategoryId
{
    public record GetProductsByCategoryIdQuery(int CategoryId) : IRequest<ApiResponse<IEnumerable<ProductResponse>>>;
}
