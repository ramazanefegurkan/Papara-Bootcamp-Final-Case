
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
using Microsoft.EntityFrameworkCore;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ProductDetailResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == query.Id,includeProperties: ["CategoryProducts", "CategoryProducts.Category"]);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            var mappedProduct = _mapper.Map<ProductDetailResponse>(product);
            return ApiResponse<ProductDetailResponse>.SuccessResult(mappedProduct);
        }
    }
}
