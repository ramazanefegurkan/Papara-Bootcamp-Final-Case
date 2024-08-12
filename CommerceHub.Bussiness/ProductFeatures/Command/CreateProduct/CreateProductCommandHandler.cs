
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Bussiness.Cache;

namespace CommerceHub.Bussiness.ProductFeatures.Command.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<ProductRequest, Product>(command.Request);

            var categories = await _unitOfWork.CategoryRepository
                .GetAll(c => command.Request.CategoryIds.Contains(c.Id));

            if (categories.Count != command.Request.CategoryIds.Count)
            {
                var notFoundIds = command.Request.CategoryIds.Except(categories.Select(c => c.Id)).ToList();
                throw new NotFoundException($"Categories not found: {string.Join(", ", notFoundIds)}");
            }

            foreach (var category in categories)
            {
                product.CategoryProducts.Add(new CategoryProduct { Product = product, Category = category });
            }


            await _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.Complete();
            await _cacheService.RemoveAsync("products");
            return ApiResponse<int>.SuccessResult(product.Id, "Product created successfully.");
        }
    }
}
