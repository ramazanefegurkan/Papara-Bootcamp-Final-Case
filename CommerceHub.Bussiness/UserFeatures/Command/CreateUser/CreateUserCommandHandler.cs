
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
using CommerceHub.Data.Enums;

namespace CommerceHub.Bussiness.UserFeatures.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserRequest, User>(command.Request);
            user.Role = UserRole.Normal;

            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.Complete();

            return ApiResponse<int>.SuccessResult(user.Id, "User created successfully.");
        }
    }
}
