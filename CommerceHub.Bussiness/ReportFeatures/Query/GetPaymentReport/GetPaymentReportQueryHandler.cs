using AutoMapper;
using CommerceHub.Bussiness.ReportFeatures.Query.CriticalStockLevelReport;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.GetPaymentReport
{
    public class GetPaymentReportQueryHandler : IRequestHandler<GetPaymentReportQuery, ApiResponse<IEnumerable<PaymentReportResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPaymentReportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PaymentReportResponse>>> Handle(GetPaymentReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetPaymentReport(request.start,request.end);
            return ApiResponse<IEnumerable<PaymentReportResponse>>.SuccessResult(report);
        }
    }
}
