
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.UpdateUser
{
    public record UpdateUserCommand(int Id, UserRequest Request) : IRequest<ApiResponse<bool>>;
}
