using CommerceHub.Schema.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Repositories.ReportRepository
{
    public interface IReportRepository
    {
        Task<IEnumerable<CriticalStockLevelResponse>> GetCriticalStockLevelReport();
        Task<IEnumerable<PaymentReportResponse>> GetPaymentReport(DateTime start, DateTime end);
        Task<IEnumerable<BestSellingProductReportResponse>> GetBestSellingProducts(DateTime start, DateTime end, int topN);
    }
}
