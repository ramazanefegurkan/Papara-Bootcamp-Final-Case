using AutoMapper;
using CommerceHub.Bussiness.Auth.Token;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Command.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(command.Id);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            _mapper.Map(command.Request, category);

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "Category updated successfully.");
        }
    }
}
