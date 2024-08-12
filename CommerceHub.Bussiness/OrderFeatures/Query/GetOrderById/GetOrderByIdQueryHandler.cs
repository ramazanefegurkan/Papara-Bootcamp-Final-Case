
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;

namespace CommerceHub.Bussiness.OrderFeatures.Query.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<OrderResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(query.Id);
            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }
            var mappedOrder = _mapper.Map<OrderResponse>(order);
            return ApiResponse<OrderResponse>.SuccessResult(mappedOrder);
        }
    }
}
