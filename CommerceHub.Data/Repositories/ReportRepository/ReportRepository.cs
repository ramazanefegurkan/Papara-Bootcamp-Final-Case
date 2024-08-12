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
            FROM  ""Products""
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
                ""Order"" o
            WHERE 
                o.""InsertDate"" BETWEEN @StartDate AND @EndDate";

            var parameters = new { StartDate = start, EndDate = end };

            return await _dapperRepository.QueryAsync<PaymentReportResponse>(sql,parameters);
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
                ""OrderDetail"" od
            INNER JOIN 
                ""Order"" o ON od.""OrderId"" = o.""Id""
            WHERE 
                o.""InsertDate"" BETWEEN @StartDate AND @EndDate
            GROUP BY 
                od.""ProductId"", od.""ProductName""
            ORDER BY 
                TotalQuantitySold DESC
            LIMIT @TopN";

            var parameters = new { StartDate = start, EndDate = end, TopN = topN };

            return await _dapperRepository.QueryAsync<BestSellingProductReportResponse>(query,parameters);
        }
    }
}
