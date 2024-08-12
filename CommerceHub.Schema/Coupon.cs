using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Schema
{
    public class CouponRequest
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class CouponResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
