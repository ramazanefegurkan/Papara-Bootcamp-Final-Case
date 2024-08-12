
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using CommerceHub.Bussiness.Cache;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ApiResponse<IEnumerable<ProductDetailResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<ApiResponse<IEnumerable<ProductDetailResponse>>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            var cacheResult = await _cacheService.GetAsync<IEnumerable<ProductDetailResponse>>("products");
            if(cacheResult != null)
            {
                return ApiResponse<IEnumerable<ProductDetailResponse>>.SuccessResult(cacheResult);
            }

            IEnumerable<Product> productList = await _unitOfWork.ProductRepository.GetAll(
                includeProperties: ["CategoryProducts", "CategoryProducts.Category"]);
           
            var mappedList = _mapper.Map<List<ProductDetailResponse>>(productList);

            if(mappedList.Count > 0)
            {
                await _cacheService.SetAsync<IEnumerable<ProductDetailResponse>>("products", mappedList,TimeSpan.FromDays(1));
            }

            return ApiResponse<IEnumerable<ProductDetailResponse>>.SuccessResult(mappedList);
        }
    }
}
