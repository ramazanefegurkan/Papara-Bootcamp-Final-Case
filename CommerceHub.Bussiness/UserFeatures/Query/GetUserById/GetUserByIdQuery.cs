
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Query.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<ApiResponse<BasicUserResponse>>;
}
