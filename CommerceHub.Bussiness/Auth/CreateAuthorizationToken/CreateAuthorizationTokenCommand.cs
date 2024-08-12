using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Auth.CreateAuthorizationToken
{
    public record CreateAuthorizationTokenCommand(AuthorizationRequest Request) : IRequest<AuthorizationResponse>;

}
