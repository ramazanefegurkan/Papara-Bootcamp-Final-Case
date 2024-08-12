using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.CriticalStockLevelReport
{
    public record GetCriticalStockLevelReportQuery : IRequest<ApiResponse<IEnumerable<CriticalStockLevelResponse>>>;
}
