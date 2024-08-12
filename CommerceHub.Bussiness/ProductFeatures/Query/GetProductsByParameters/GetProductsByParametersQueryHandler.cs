using AutoMapper;
using CommerceHub.Bussiness.ProductFeatures.Query.GetAllProducts;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByParameters
{
    public class GetProductsByParametersQueryHandler : IRequestHandler<GetProductsByParametersQuery, ApiResponse<IEnumerable<ProductDetailResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProductsByParametersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductDetailResponse>>> Handle(GetProductsByParametersQuery query, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<Product>(true);

            if (!string.IsNullOrEmpty(query.Name))
            {
                predicate = predicate.And(p => p.Name.Contains(query.Name));
            }

            if (query.Status.HasValue)
            {
                predicate = predicate.And(p => p.Status == query.Status.Value);
            }

            if (query.MinPrice.HasValue)
            {
                predicate = predicate.And(p => p.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                predicate = predicate.And(p => p.Price <= query.MaxPrice.Value);
            }

            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null;

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "name":
                        orderBy = query.SortOrder == "desc"
                            ? (q => q.OrderByDescending(p => p.Name))
                            : (q => q.OrderBy(p => p.Name));
                        break;
                    case "price":
                        orderBy = query.SortOrder == "desc"
                            ? (q => q.OrderByDescending(p => p.Price))
                            : (q => q.OrderBy(p => p.Price));
                        break;
                    case "status":
                        orderBy = query.SortOrder == "desc"
                            ? (q => q.OrderByDescending(p => p.Status))
                            : (q => q.OrderBy(p => p.Status));
                        break;
                    default:
                        break;
                }
            }

            var products = await _unitOfWork.ProductRepository.GetAll(predicate, orderBy, includeProperties: ["CategoryProducts", "CategoryProducts.Category"]);

            var productResponses = _mapper.Map<List<ProductDetailResponse>>(products);
            return ApiResponse<IEnumerable<ProductDetailResponse>>.SuccessResult(productResponses);
        }

    }
}
