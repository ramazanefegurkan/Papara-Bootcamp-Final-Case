
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Bussiness.Exceptions;

namespace CommerceHub.Bussiness.CouponFeatures.Command.DeleteCoupon
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteCouponCommand command, CancellationToken cancellationToken)
        {
            var coupon = await _unitOfWork.CouponRepository.GetById(command.Id);
            if (coupon == null)
            {
                throw new NotFoundException("Coupon not found");
            }

            var couponUsedOrders = await _unitOfWork.OrderRepository.GetAll(x => x.CouponId == command.Id);

            if(couponUsedOrders.Any())
            {
                throw new EntityHasDependenciesException("Coupon cannot be deleted as it is used in orders.");
            }

            await _unitOfWork.CouponRepository.SoftDelete(command.Id);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "Coupon deleted successfully.");
        }
    }
}
