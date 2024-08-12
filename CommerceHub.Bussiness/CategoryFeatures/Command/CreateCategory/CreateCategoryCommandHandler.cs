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

namespace CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<CategoryRequest, Category>(command.Request);
            await _unitOfWork.CategoryRepository.Insert(mapped);
            await _unitOfWork.Complete();

            return ApiResponse<int>.SuccessResult(mapped.Id, "Category created successfully.");
        }

    }
}
