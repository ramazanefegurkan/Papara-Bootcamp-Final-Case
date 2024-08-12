
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
using CommerceHub.Data.Domain;

namespace CommerceHub.Bussiness.CouponFeatures.Command.UpdateCoupon
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateCouponCommand command, CancellationToken cancellationToken)
        {
            var coupon = await _unitOfWork.CouponRepository.GetById(command.Id);
            if (coupon == null)
            {
                throw new NotFoundException("Coupon not found");
            }

            _mapper.Map(command.Request, coupon);
            
            _unitOfWork.CouponRepository.Update(coupon);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "Coupon updated successfully.");
        }
    }
}
