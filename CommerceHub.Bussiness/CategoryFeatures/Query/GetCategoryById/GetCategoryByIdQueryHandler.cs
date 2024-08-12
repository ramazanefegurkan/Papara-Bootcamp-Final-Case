using AutoMapper;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Query.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse<CategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(query.Id);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }
            var mappedCategory = _mapper.Map<CategoryResponse>(category);
            return ApiResponse<CategoryResponse>.SuccessResult(mappedCategory);
        }
    }
}
