
using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.CouponFeatures.Command;
using CommerceHub.Bussiness.CouponFeatures.Command.CreateCoupon;
using CommerceHub.Bussiness.CouponFeatures.Command.DeleteCoupon;
using CommerceHub.Bussiness.CouponFeatures.Command.UpdateCoupon;
using CommerceHub.Bussiness.CouponFeatures.Query;
using CommerceHub.Bussiness.CouponFeatures.Query.GetAllCoupons;
using CommerceHub.Bussiness.CouponFeatures.Query.GetCouponById;
using CommerceHub.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var operation = new GetCouponByIdQuery(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var operation = new GetAllCouponsQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] CouponRequest value)
        {
            var operation = new CreateCouponCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] CouponRequest value)
        {
            var operation = new UpdateCouponCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = new DeleteCouponCommand(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
