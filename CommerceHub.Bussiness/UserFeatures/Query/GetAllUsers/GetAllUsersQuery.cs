
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Domain;

namespace CommerceHub.Bussiness.UserFeatures.Query.GetAllUsers
{
	public record GetAllUsersQuery : IRequest<ApiResponse<IEnumerable<BasicUserResponse>>>;
}
