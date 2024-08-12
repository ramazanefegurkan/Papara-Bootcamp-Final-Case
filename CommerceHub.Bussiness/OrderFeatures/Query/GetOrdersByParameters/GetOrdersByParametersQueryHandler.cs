using AutoMapper;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using LinqKit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetOrdersByParameters
{
    public class GetOrdersByParametersQueryHandler : IRequestHandler<GetOrdersByParametersQuery, ApiResponse<IEnumerable<OrderResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetOrdersByParametersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<OrderResponse>>> Handle(GetOrdersByParametersQuery query, CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<Order>(true);

            if (!string.IsNullOrEmpty(query.OrderNumber))
            {
                predicate = predicate.And(p => p.OrderNumber.Contains(query.OrderNumber));
            }

            if (query.MinAmount.HasValue)
            {
                predicate = predicate.And(p => p.TotalAmount >= query.MinAmount.Value);
            }

            if (query.MaxAmount.HasValue)
            {
                predicate = predicate.And(p => p.TotalAmount <= query.MaxAmount.Value);
            }

            Func<IQueryable<Order>, IOrderedQueryable<Order>>? orderBy = null;

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "orderNumber":
                        orderBy = query.SortOrder == "desc"
                            ? (q => q.OrderByDescending(p => p.OrderNumber))
                            : (q => q.OrderBy(p => p.OrderNumber));
                        break;
                    case "totalAmount":
                        orderBy = query.SortOrder == "desc"
                            ? (q => q.OrderByDescending(p => p.TotalAmount))
                            : (q => q.OrderBy(p => p.TotalAmount));
                        break;
                    default:
                        break;
                }
            }

            var orders = await _unitOfWork.OrderRepository.GetAll(predicate, orderBy, includeProperties: ["OrderDetails"]);

            var ordersResponses = _mapper.Map<List<OrderResponse>>(orders);
            return ApiResponse<IEnumerable<OrderResponse>>.SuccessResult(ordersResponses);
        }
    }
}
