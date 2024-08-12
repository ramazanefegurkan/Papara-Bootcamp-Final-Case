using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.CreateAdminUser
{
    public record CreateAdminUserCommand(UserRequest Request) : IRequest<ApiResponse<int>>;
}
