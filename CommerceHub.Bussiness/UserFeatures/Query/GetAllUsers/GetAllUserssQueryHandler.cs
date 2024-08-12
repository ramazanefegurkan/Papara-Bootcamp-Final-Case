
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Data.Domain;

namespace CommerceHub.Bussiness.UserFeatures.Query.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<IEnumerable<BasicUserResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<BasicUserResponse>>> Handle(GetAllUsersQuery query,CancellationToken cancellationToken)
        {
            IEnumerable<User> userList = await _unitOfWork.UserRepository.GetAll();
            var mappedList = _mapper.Map<IEnumerable<BasicUserResponse>>(userList);
            return ApiResponse<IEnumerable<BasicUserResponse>>.SuccessResult(mappedList);
        }
    }
}
