using CommerceHub.Base.Entity;
using CommerceHub.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Domain
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }
        public int StockQuantity { get; set; }
        public int CriticalStockLevel { get; set; }
        public ProductStatus Status { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new HashSet<CategoryProduct>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
