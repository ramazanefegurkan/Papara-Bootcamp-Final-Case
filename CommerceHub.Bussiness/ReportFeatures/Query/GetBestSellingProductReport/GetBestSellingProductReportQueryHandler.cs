using AutoMapper;
using CommerceHub.Bussiness.ReportFeatures.Query.GetPaymentReport;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.BestSellingProductReport
{
    public class GetBestSellingProductReportQueryHandler : IRequestHandler<GetBestSellingProductReportQuery, ApiResponse<IEnumerable<BestSellingProductReportResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBestSellingProductReportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<BestSellingProductReportResponse>>> Handle(GetBestSellingProductReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetBestSellingProducts(request.Start, request.End,request.TopN);
            return ApiResponse<IEnumerable<BestSellingProductReportResponse>>.SuccessResult(report);
        }
    }
}
