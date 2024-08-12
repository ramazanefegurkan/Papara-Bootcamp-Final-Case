using AutoMapper;
using CommerceHub.Base.Helper.PasswordHasher;
using CommerceHub.Bussiness.Auth.Token;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Auth.CreateAuthorizationToken
{
    public class CreateAuthorizationTokenCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, AuthorizationResponse>
    {
        private ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateAuthorizationTokenCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<AuthorizationResponse> Handle(CreateAuthorizationTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefault(x => x.UserName == command.Request.UserName);
            if (user == null)
                throw new InvalidCredentialsException("Invalid user informations. Check your username or password");

            if (!PasswordHasher.VerifyPassword(user.PasswordHash, command.Request.Password))
                throw new InvalidCredentialsException("Invalid user informations. Check your username or password");

            var token = _tokenService.GetToken(user);
            AuthorizationResponse response = new AuthorizationResponse()
            {
                AccessToken = token,
            };

            return response;
        }
    }
}
