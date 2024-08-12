
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

namespace CommerceHub.Bussiness.UserFeatures.Query.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<BasicUserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<BasicUserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(query.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var mappedUser = _mapper.Map<BasicUserResponse>(user);
            return ApiResponse<BasicUserResponse>.SuccessResult(mappedUser);
        }
    }
}
