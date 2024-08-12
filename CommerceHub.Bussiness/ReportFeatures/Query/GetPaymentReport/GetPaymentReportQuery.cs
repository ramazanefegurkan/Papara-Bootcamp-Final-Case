using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.GetPaymentReport
{
    public record GetPaymentReportQuery(DateTime start,DateTime end) : IRequest<ApiResponse<IEnumerable<PaymentReportResponse>>>;
}
