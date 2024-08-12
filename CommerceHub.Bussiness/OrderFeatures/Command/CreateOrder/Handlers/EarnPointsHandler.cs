using CommerceHub.Base;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class EarnPointsHandler : IOrderHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;
        public int Order => 9;

        public EarnPointsHandler(IUnitOfWork unitOfWork, ISessionContext sessionContext)
        {
            _unitOfWork = unitOfWork;
            _sessionContext = sessionContext;
        }

        public async Task Handle(OrderContext context)
        {
            var user = await _unitOfWork.UserRepository.GetById(_sessionContext.Session.UserId);

            decimal totalPointsEarned = 0;
            decimal totalAmount = context.Order.TotalAmount;
            decimal paidAmount = context.Order.PaidAmount;

            foreach (var item in context.Order.OrderDetails)
            {
                var product = context.Products.First(x => x.Id == item.ProductId);

                decimal itemTotalPrice = item.Price * item.Quantity;
                decimal itemPaidAmount = (itemTotalPrice / totalAmount) * paidAmount;
                decimal paidPerItem = itemPaidAmount / item.Quantity;

                // Puan kazanımı
                var pointsEarned = paidPerItem * product.RewardPointsPercentage / 100;
                if (pointsEarned > product.MaxRewardPoints)
                {
                    pointsEarned = product.MaxRewardPoints;
                }

                totalPointsEarned += pointsEarned * item.Quantity;
            }

            user.WalletBalance += totalPointsEarned;
            context.TotalPointsEarned = totalPointsEarned;
            context.Order.EarnedPoints = totalPointsEarned;
            _unitOfWork.UserRepository.Update(user);

            await Task.CompletedTask;
        }
    }
}
