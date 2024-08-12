using AutoMapper;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.Enums;
using CommerceHub.Data.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CommerceHub.Bussiness.ProductFeatures.Command.UpdateProductStocck
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateProductStockCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(command.Id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            product.StockQuantity = command.Request.NewStockQuantity;
            product.Status = product.StockQuantity == 0 ? ProductStatus.OutOfStock : product.Status;

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResult(true,"Stock updated succesfully");
        }
    }
}
