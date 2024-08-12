using CommerceHub.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Domain
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } 
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal UsedPoints { get; set; }
        public decimal EarnedPoints { get; set; }
        public string? CouponCode { get; set; }
        public int? CouponId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Coupon UsedCoupon { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
