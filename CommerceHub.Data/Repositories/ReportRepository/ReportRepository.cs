using CommerceHub.Data.Repositories.DapperRepository;
using CommerceHub.Schema.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDapperRepository _dapperRepository;

        public ReportRepository(IDapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        public async Task<IEnumerable<CriticalStockLevelResponse>> GetCriticalStockLevelReport()
        {
            var sql = @"
            SELECT ""Id"",""Name"",""StockQuantity"",""CriticalStockLevel""
            FROM public.""Products""
            WHERE ""StockQuantity"" <= ""CriticalStockLevel""";
           
            return await _dapperRepository.QueryAsync<CriticalStockLevelResponse>(sql);
        }

        public async Task<IEnumerable<PaymentReportResponse>> GetPaymentReport(DateTime start, DateTime end)
        {
            var sql = @"
            SELECT 
                COUNT(*) as OrderCount,
                SUM(o.""TotalAmount"") as TotalAmount,
                SUM(o.""PaidAmount"") as PaidAmount,
                SUM(o.""CouponAmount"") as CouponAmount,
                SUM(o.""UsedPoints"") as UsedPoints
            FROM 
                ""Orders"" o
            WHERE 
                o.""CreatedDate"" BETWEEN @Start AND @End";

            var parameters = new { Start = start, End = end };

            return await _dapperRepository.QueryAsync<PaymentReportResponse>(sql);
        }

        public async Task<IEnumerable<BestSellingProductReportResponse>> GetBestSellingProducts(DateTime start, DateTime end, int topN)
        {
            var query = @"
            SELECT 
                od.""ProductId"",
                od.""ProductName"",
                SUM(od.""Quantity"") AS TotalQuantitySold,
                SUM(od.""Quantity"" * od.""Price"") AS TotalRevenue
            FROM 
                ""OrderDetails"" od
            INNER JOIN 
                ""Orders"" o ON od.""OrderId"" = o.""Id""
            WHERE 
                o.""CreatedDate"" BETWEEN @Start AND @End
            GROUP BY 
                od.""ProductId"", od.""Name""
            ORDER BY 
                TotalQuantitySold DESC
            LIMIT @TopN";

            var parameters = new { Start = start, End = end, TopN = topN };

            return await _dapperRepository.QueryAsync<BestSellingProductReportResponse>(query);
        }
    }
}
