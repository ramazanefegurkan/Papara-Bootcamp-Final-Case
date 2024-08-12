
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

namespace CommerceHub.Bussiness.UserFeatures.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(command.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            await _unitOfWork.UserRepository.SoftDelete(command.Id);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "User deleted successfully.");
        }
    }
}
