using AutoMapper;
using CommerceHub.Bussiness.UserFeatures.Command.CreateUser;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Enums;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.CreateAdminUser
{
    public class CreateAdminUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAdminUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserRequest, User>(command.Request);
            user.Role = UserRole.Admin;

            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.Complete();

            return ApiResponse<int>.SuccessResult(user.Id, "User created successfully.");
        }
    }
}
