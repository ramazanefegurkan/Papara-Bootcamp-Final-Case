
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
using CommerceHub.Data.Domain;
using Microsoft.EntityFrameworkCore;
using CommerceHub.Data.Enums;
using CommerceHub.Bussiness.Cache;

namespace CommerceHub.Bussiness.ProductFeatures.Command.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == command.Id,includeProperties: ["CategoryProducts"]);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            if(command.Request.Status == ProductStatus.OnSale && product.StockQuantity !> 0)
            {
                throw new InvalidProductStatusException("Product status cannot be OnSale if stock quantity is 0");
            }

            _mapper.Map(command.Request, product);

            var newCategoryIds = command.Request.CategoryIds;
            var existingCategoryIds = product.CategoryProducts.Select(cp => cp.CategoryId).ToList();

            var categoriesToAdd = newCategoryIds.Except(existingCategoryIds).ToList();
            var categoriesToRemove = existingCategoryIds.Except(newCategoryIds).ToList();

            foreach (var categoryId in categoriesToAdd)
            {
                var category = await _unitOfWork.CategoryRepository.GetById(categoryId);
                if (category == null)
                {
                    throw new NotFoundException($"Category with ID {categoryId} not found");
                }
                product.CategoryProducts.Add(new CategoryProduct { ProductId = product.Id, CategoryId = category.Id });
            }

            product.CategoryProducts = product.CategoryProducts
                .Where(cp => !categoriesToRemove.Contains(cp.CategoryId))
                .ToList();

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Complete();
            await _cacheService.RemoveAsync("products");
            return ApiResponse<bool>.SuccessResult(true, "Product updated successfully.");
        }
    }
}
