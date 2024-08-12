
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.DeleteUser
{
    public record DeleteUserCommand(int Id) : IRequest<ApiResponse<bool>>;
}
