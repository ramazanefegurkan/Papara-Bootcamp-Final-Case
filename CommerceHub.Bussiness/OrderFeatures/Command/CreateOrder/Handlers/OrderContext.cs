using CommerceHub.Data.Domain;
using CommerceHub.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class OrderContext
    {
        public OrderRequest OrderRequest { get; set; }
        public Order Order { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal TotalPointsEarned { get; set; }
        public decimal PaidAmount { get; set; }
        public List<Product> Products { get; set; }
    }
}
