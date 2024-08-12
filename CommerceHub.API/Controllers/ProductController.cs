
using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.ProductFeatures.Command;
using CommerceHub.Bussiness.ProductFeatures.Command.CreateProduct;
using CommerceHub.Bussiness.ProductFeatures.Command.DeleteProduct;
using CommerceHub.Bussiness.ProductFeatures.Command.UpdateProduct;
using CommerceHub.Bussiness.ProductFeatures.Command.UpdateProductStocck;
using CommerceHub.Bussiness.ProductFeatures.Query;
using CommerceHub.Bussiness.ProductFeatures.Query.GetAllProducts;
using CommerceHub.Bussiness.ProductFeatures.Query.GetProductById;
using CommerceHub.Bussiness.ProductFeatures.Query.GetProductsByCategoryId;
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
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var operation = new GetProductByIdQuery(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var operation = new GetAllProductsQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? name, [FromQuery] ProductStatus? status, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] string? sortBy, [FromQuery] string? sortOrder)
        {
            var query = new GetProductsByParametersQuery(name, status, minPrice, maxPrice, sortBy, sortOrder);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("fromCategory/{categoryId}")]
        [Authorize]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var query = new GetProductsByCategoryIdQuery(categoryId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] ProductRequest value)
        {
            var operation = new CreateProductCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] BasicProductRequest value)
        {
            var operation = new UpdateProductCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = new DeleteProductCommand(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{id}/updateStock")]
        public async Task<IActionResult> UpdateProductStock(int id, [FromBody] UpdateProductStockRequest value)
        {
            var operation = new UpdateProductStockCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
