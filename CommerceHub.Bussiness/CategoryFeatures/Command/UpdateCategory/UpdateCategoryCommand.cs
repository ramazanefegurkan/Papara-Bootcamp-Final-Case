using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Command.UpdateCategory
{
    public record UpdateCategoryCommand(int Id, CategoryRequest Request) : IRequest<ApiResponse<bool>>;
}
