using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Query.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<ApiResponse<CategoryResponse>>;
}
