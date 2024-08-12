using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Query.GetUserPoints
{
    public record GetUserPointsQuery : IRequest<ApiResponse<decimal>>;

}
