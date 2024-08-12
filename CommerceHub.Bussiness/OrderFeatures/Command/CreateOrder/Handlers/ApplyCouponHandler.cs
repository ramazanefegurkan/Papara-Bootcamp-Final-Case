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
    public class ApplyCouponHandler : IOrderHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;
        public int Order => 5;
        public ApplyCouponHandler(IUnitOfWork unitOfWork,ISessionContext sessionContext)
        {
            _unitOfWork = unitOfWork;
            _sessionContext = sessionContext;
        }

        public async Task Handle(OrderContext context)
        {
            if (!string.IsNullOrEmpty(context.OrderRequest.CouponCode))
            {
                var coupon = await _unitOfWork.CouponRepository.FirstOrDefault(x => x.Code == context.OrderRequest.CouponCode);
                if (coupon == null)
                    throw new NotFoundException("Coupon not found");

                if (coupon.ExpiryDate < DateTime.Now)
                    throw new CouponExpiredException("Coupon expired on: " + coupon.ExpiryDate.ToString("dd/MM/yyyy"));

                var usedCoupons = await _unitOfWork.OrderRepository.GetAll(x => x.CouponId == coupon.Id && x.UserId == _sessionContext.Session.UserId);
                if (usedCoupons.Any())
                    throw new CouponAlreadyUsedException("Coupon already used");

                context.Order.CouponId = coupon.Id;
                context.Order.CouponCode = coupon.Code;
                context.Order.CouponAmount = coupon.Amount;
                context.RemainingAmount -= coupon.Amount;

            }

            await Task.CompletedTask;
        }
    }
}
