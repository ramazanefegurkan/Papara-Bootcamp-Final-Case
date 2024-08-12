
using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.OrderFeatures.Command;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder;
using CommerceHub.Bussiness.OrderFeatures.Command.DeleteOrder;
using CommerceHub.Bussiness.OrderFeatures.Command.UpdateOrder;
using CommerceHub.Bussiness.OrderFeatures.Query;
using CommerceHub.Bussiness.OrderFeatures.Query.GetAllOrders;
using CommerceHub.Bussiness.OrderFeatures.Query.GetOrderById;
using CommerceHub.Bussiness.OrderFeatures.Query.GetOrdersByParameters;
using CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByParameters;
using CommerceHub.Data.Enums;
using CommerceHub.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var operation = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var operation = new GetAllOrdersQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? orderNumber, [FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] string? sortBy, [FromQuery] string? sortOrder)
        {
            var query = new GetOrdersByParametersQuery(orderNumber, minAmount, maxAmount, sortBy, sortOrder);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest value)
        {
            var operation = new CreateOrderCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderRequest value)
        {
            var operation = new UpdateOrderCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = new DeleteOrderCommand(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
