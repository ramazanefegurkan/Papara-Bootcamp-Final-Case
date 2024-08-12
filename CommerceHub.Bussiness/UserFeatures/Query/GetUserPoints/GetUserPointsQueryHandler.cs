using CommerceHub.Base;
using CommerceHub.Data.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Query.GetUserPoints
{
    public class GetUserPointsQueryHandler : IRequestHandler<GetUserPointsQuery, ApiResponse<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;

        public GetUserPointsQueryHandler(IUnitOfWork unitOfWork, ISessionContext sessionContext)
        {
            _unitOfWork = unitOfWork;
            _sessionContext = sessionContext;

        }

        public async Task<ApiResponse<decimal>> Handle(GetUserPointsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefault(x => x.Id == _sessionContext.Session.UserId);
            return ApiResponse<decimal>.SuccessResult(user.WalletBalance);
        }
    }
}
