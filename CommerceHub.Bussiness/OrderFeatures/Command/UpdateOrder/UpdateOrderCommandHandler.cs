
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Bussiness.Exceptions;

namespace CommerceHub.Bussiness.OrderFeatures.Command.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(command.Id);
            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }
            
            //Update Order

            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "Order uptaded successfully.");
        }
    }
}
