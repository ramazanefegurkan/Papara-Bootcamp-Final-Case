
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

namespace CommerceHub.Bussiness.UserFeatures.Command.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetById(command.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            
            _mapper.Map(command.Request, user);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Complete();
            return ApiResponse<bool>.SuccessResult(true, "User updated successfully.");
        }
    }
}
