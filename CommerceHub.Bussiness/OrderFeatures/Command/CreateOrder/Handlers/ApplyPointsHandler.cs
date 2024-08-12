using CommerceHub.Base;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class ApplyPointsHandler : IOrderHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;
        public int Order => 6;
        public ApplyPointsHandler(IUnitOfWork unitOfWork, ISessionContext sessionContext)
        {
            _unitOfWork = unitOfWork;
            _sessionContext = sessionContext;
        }

        public async Task Handle(OrderContext context)
        {
            if (context.RemainingAmount <= 0)
                return;

            var user = await _unitOfWork.UserRepository.GetById(_sessionContext.Session.UserId);
            if (context.OrderRequest.PointsToUse > 0)
            {
                if (context.OrderRequest.PointsToUse > user.WalletBalance)
                {
                    throw new InsufficientPointsException($"User does not have enough points. Available: {user.WalletBalance}, Requested: {context.OrderRequest.PointsToUse}");
                }
                context.Order.UsedPoints = context.OrderRequest.PointsToUse;
                context.RemainingAmount -= context.OrderRequest.PointsToUse;
                user.WalletBalance -= context.OrderRequest.PointsToUse;
                _unitOfWork.UserRepository.Update(user);
            }

            await Task.CompletedTask;
        }
    }
}
