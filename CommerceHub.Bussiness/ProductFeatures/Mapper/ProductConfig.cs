using AutoMapper;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Enums;
using CommerceHub.Schema;

namespace CommerceHub.Bussiness.ProductFeatures.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom<StatusResolver>());
            CreateMap<BasicProductRequest, Product>();

            CreateMap<Product, ProductResponse>();
            CreateMap<Product, ProductDetailResponse>().ForMember(
                dest => dest.Categories, opt => opt.MapFrom(src => src.CategoryProducts.Select(cp => cp.Category).ToList()));

        }

        public class StatusResolver : IValueResolver<ProductRequest, Product, ProductStatus>
        {
            public ProductStatus Resolve(ProductRequest source, Product destination, ProductStatus destMember, ResolutionContext context)
            {
                if (source.Status == ProductStatus.OutOfSale)
                {
                    return ProductStatus.OutOfSale;
                }

                return source.StockQuantity > 0 ? ProductStatus.OnSale : ProductStatus.OutOfStock;
            }
        }
    }
}
