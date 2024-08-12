
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
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResponse<IEnumerable<OrderResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<OrderResponse>>> Handle(GetAllOrdersQuery query,CancellationToken cancellationToken)
        {
            IEnumerable<Order> orderList = await _unitOfWork.OrderRepository.GetAll(
                includeProperties: ["OrderDetails"]);

            var mappedList = _mapper.Map<IEnumerable<OrderResponse>>(orderList);
            return ApiResponse<IEnumerable<OrderResponse>>.SuccessResult(mappedList);
        }
    }
}
