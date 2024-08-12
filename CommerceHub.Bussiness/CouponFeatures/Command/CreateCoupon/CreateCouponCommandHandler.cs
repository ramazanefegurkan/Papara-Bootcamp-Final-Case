
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using CommerceHub.Base;
using CommerceHub.Bussiness.Exceptions;

namespace CommerceHub.Bussiness.CouponFeatures.Command.CreateCoupon
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISessionContext _sessionContext;

        public CreateCouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISessionContext sessionContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sessionContext = sessionContext;
        }

        public async Task<ApiResponse<int>> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
        {
            var existingCoupon = await _unitOfWork.CouponRepository.FirstOrDefault(x => x.Code == command.Request.Code);
            if (existingCoupon != null)
                throw new CouponAlreadyExistsException();

            var coupon = _mapper.Map<CouponRequest, Coupon>(command.Request);
            coupon.AdminUserId = _sessionContext.Session.UserId;

            await _unitOfWork.CouponRepository.Insert(coupon);
            await _unitOfWork.Complete();

            return ApiResponse<int>.SuccessResult(coupon.Id, "Coupon created successfully.");
        }
    }
}
