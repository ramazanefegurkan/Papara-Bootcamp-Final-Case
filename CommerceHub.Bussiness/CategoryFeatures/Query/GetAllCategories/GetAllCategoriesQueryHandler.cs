using AutoMapper;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommerceHub.Bussiness.CategoryFeatures.Query.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ApiResponse<IEnumerable<CategoryResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<CategoryResponse>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Category> categoryList = await _unitOfWork.CategoryRepository.GetAll();
            var mappedList = _mapper.Map<IEnumerable<CategoryResponse>>(categoryList);
            return ApiResponse<IEnumerable<CategoryResponse>>.SuccessResult(mappedList);
        }
    }
}
