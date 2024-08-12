using CommerceHub.Bussiness.ReportFeatures.Query.BestSellingProductReport;
using CommerceHub.Bussiness.ReportFeatures.Query.CriticalStockLevelReport;
using CommerceHub.Bussiness.ReportFeatures.Query.GetPaymentReport;
using CommerceHub.Bussiness.UserFeatures.Query.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("criticalStockLevel")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetCriticalStockLevelReport()
        {
            var operation = new GetCriticalStockLevelReportQuery();
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet("bestSellingProduct")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetBestSellingProductReport(DateTime start,DateTime end,int TopN)
        {
            var operation = new GetBestSellingProductReportQuery(start,end,TopN);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }

        [HttpGet("payment")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetPaymentReport(DateTime start, DateTime end)
        {
            var operation = new GetPaymentReportQuery(start, end);
            var result = await _mediator.Send(operation);
            return Ok(result);
        }
    }
}
