using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory
{
    public record CreateCategoryCommand(CategoryRequest Request) : IRequest<ApiResponse<int>>;
}
