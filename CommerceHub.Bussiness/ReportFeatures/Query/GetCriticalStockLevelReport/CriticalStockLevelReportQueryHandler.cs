using AutoMapper;
using CommerceHub.Bussiness.OrderFeatures.Query.GetAllOrders;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using CommerceHub.Schema.Report;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ReportFeatures.Query.CriticalStockLevelReport
{
    public class GetCriticalStockLevelReportQueryHandler : IRequestHandler<GetCriticalStockLevelReportQuery, ApiResponse<IEnumerable<CriticalStockLevelResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCriticalStockLevelReportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CriticalStockLevelResponse>>> Handle(GetCriticalStockLevelReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _unitOfWork.ReportRepository.GetCriticalStockLevelReport();
            return ApiResponse<IEnumerable<CriticalStockLevelResponse>>.SuccessResult(report);
        }
    }
}
