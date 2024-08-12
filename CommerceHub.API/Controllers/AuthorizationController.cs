using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorizationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest value)
        {
            var operation = new CreateAuthorizationTokenCommand(value);
            var result = await mediator.Send(operation);
            return ApiResponse<AuthorizationResponse>.SuccessResult(result) ;
        }

    }
}
