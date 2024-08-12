
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
using CommerceHub.Bussiness.Cache;

namespace CommerceHub.Bussiness.ProductFeatures.Command.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(command.Id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            await _unitOfWork.ProductRepository.SoftDelete(command.Id);
            await _unitOfWork.Complete();
            await _cacheService.RemoveAsync("products");
            return ApiResponse<bool>.SuccessResult(true, "Product deleted successfully.");
        }
    }
}
