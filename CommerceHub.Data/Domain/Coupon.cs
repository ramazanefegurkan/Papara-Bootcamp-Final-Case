using CommerceHub.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Domain
{
    public class Coupon : BaseEntity
    {
        public string Code { get; set; } 
        public decimal Amount { get; set; }
        public DateTime ExpiryDate { get; set; } 
        public int AdminUserId { get; set; } 

        public virtual User AdminUser { get; set; }
        public ICollection<Order> UsedOrders { get; set; } = new List<Order>();
    }
}
