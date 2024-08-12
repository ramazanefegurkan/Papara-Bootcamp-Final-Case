using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Schema.Report
{
    public class PaymentReportResponse
    {
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal UsedPoints { get; set; }
    }
}
