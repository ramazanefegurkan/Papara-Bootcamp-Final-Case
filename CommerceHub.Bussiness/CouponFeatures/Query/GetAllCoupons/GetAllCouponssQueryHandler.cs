
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Data.Domain;

namespace CommerceHub.Bussiness.CouponFeatures.Query.GetAllCoupons
{
    public class GetAllCouponsQueryHandler : IRequestHandler<GetAllCouponsQuery, ApiResponse<IEnumerable<CouponResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCouponsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<CouponResponse>>> Handle(GetAllCouponsQuery query,CancellationToken cancellationToken)
        {
            IEnumerable<Coupon> couponList = await _unitOfWork.CouponRepository.GetAll();
            var mappedList = _mapper.Map<IEnumerable<CouponResponse>>(couponList);
            return ApiResponse<IEnumerable<CouponResponse>>.SuccessResult(mappedList);
        }
    }
}
