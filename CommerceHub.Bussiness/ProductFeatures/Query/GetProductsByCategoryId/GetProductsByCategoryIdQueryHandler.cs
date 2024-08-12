using AutoMapper;
using CommerceHub.Bussiness.Cache;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByCategoryId
{
    public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, ApiResponse<IEnumerable<ProductResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsByCategoryIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<ApiResponse<IEnumerable<ProductResponse>>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository
                 .GetAll(p => p.CategoryProducts.Any(cp => cp.CategoryId == query.CategoryId));
            var mappedProducts = _mapper.Map<List<ProductResponse>>(products);
            return ApiResponse<IEnumerable<ProductResponse>>.SuccessResult(mappedProducts);
        }
    }
}
