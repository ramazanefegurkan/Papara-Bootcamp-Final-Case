using AutoMapper;
using CommerceHub.Base.Helper.PasswordHasher;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Enums;
using CommerceHub.Schema;

namespace CommerceHub.Bussiness.UserFeatures.Mapper
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<User, BasicUserResponse>();
            CreateMap<User, AdminUserResponse>();
            CreateMap<UserRequest, User>()
                       .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom<PasswordHashResolver>());
        }

        public class PasswordHashResolver : IValueResolver<UserRequest, User, string>
        {
            public string Resolve(UserRequest source, User destination, string destMember, ResolutionContext context)
            {
                return PasswordHasher.HashPassword(source.Password);
            }
        }
    }
}
