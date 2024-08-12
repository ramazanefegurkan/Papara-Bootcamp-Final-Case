using AutoMapper;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Enums;
using CommerceHub.Schema;

namespace CommerceHub.Bussiness.CouponFeatures.Mapper
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Coupon, CouponResponse>();
            CreateMap<CouponRequest, Coupon>();
        }
    }
}
