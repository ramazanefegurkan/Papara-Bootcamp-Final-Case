
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;

namespace CommerceHub.Bussiness.CouponFeatures.Query.GetCouponById
{
    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, ApiResponse<CouponResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCouponByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<CouponResponse>> Handle(GetCouponByIdQuery query, CancellationToken cancellationToken)
        {
            var coupon = await _unitOfWork.CouponRepository.GetById(query.Id);
            if (coupon == null)
            {
                throw new NotFoundException("Coupon not found");
            }
            var mappedCoupon = _mapper.Map<CouponResponse>(coupon);
            return ApiResponse<CouponResponse>.SuccessResult(mappedCoupon);
        }
    }
}
