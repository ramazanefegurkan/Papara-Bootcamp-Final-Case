using AutoMapper;
using CommerceHub.Bussiness.Auth.Token;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Command.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<bool>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(command.Id);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            var productsInCategory = await _unitOfWork.ProductRepository
                .GetAll(p => p.CategoryProducts.Any(cp => cp.CategoryId == command.Id));

            if (productsInCategory.Any())
            {
                throw new EntityHasDependenciesException("Category has products. Please remove products first.");
            }

            await _unitOfWork.CategoryRepository.Delete(command.Id);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "Category deleted successfully.");
        }
    }
}
