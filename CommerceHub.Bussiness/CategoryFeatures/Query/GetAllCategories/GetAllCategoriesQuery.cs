using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Query.GetAllCategories
{
    public record GetAllCategoriesQuery : IRequest<ApiResponse<IEnumerable<CategoryResponse>>>;
}
