using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.BestSellingProductReport
{
    public record GetBestSellingProductReportQuery(DateTime Start, DateTime End,int TopN) : IRequest<ApiResponse<IEnumerable<BestSellingProductReportResponse>>>;
}
