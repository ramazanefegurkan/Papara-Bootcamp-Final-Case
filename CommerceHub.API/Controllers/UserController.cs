
using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.UserFeatures.Command;
using CommerceHub.Bussiness.UserFeatures.Command.CreateUser;
using CommerceHub.Bussiness.UserFeatures.Command.DeleteUser;
using CommerceHub.Bussiness.UserFeatures.Command.UpdateUser;
using CommerceHub.Bussiness.UserFeatures.Query;
using CommerceHub.Bussiness.UserFeatures.Query.GetAllUsers;
using CommerceHub.Bussiness.UserFeatures.Query.GetUserById;
using CommerceHub.Bussiness.UserFeatures.Query.GetUserPoints;
using CommerceHub.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var operation = new GetUserByIdQuery(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var operation = new GetAllUsersQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequest value)
        {
            var operation = new CreateUserCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] UserRequest value)
        {
            var operation = new UpdateUserCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = new DeleteUserCommand(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpPost("admin")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateAdminUser([FromBody] UserRequest value)
        {
            var operation = new CreateUserCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpGet("getUserPoints")]
        [Authorize]
        public async Task<IActionResult> GetUserPoints()
        {
            var operation = new GetUserPointsQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
