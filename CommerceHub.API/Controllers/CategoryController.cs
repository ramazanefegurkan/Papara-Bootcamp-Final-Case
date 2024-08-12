using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.CategoryFeatures.Command;
using CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Command.DeleteCategory;
using CommerceHub.Bussiness.CategoryFeatures.Command.UpdateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Query;
using CommerceHub.Bussiness.CategoryFeatures.Query.GetAllCategories;
using CommerceHub.Bussiness.CategoryFeatures.Query.GetCategoryById;
using CommerceHub.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var operation = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var operation = new GetAllCategoriesQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] CategoryRequest value)
        {
            var operation = new CreateCategoryCommand(value);
            var result = await _mediator.Send(operation);
            return CreatedAtAction(nameof(Get), new { id = result.Data }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryRequest value)
        {
            var operation = new UpdateCategoryCommand(id, value);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
