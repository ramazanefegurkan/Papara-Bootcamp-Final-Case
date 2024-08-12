using AutoMapper;
using CommerceHub.Data.Domain;
using CommerceHub.Schema;

namespace CommerceHub.Bussiness.CategoryFeatures.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryDetailResponse>().ReverseMap();
        }
    }
}
