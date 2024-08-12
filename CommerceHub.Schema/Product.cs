using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Enums;

namespace CommerceHub.Schema
{
    public class BasicProductRequest 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }
        public int CriticalStockLevel { get; set; }
        public List<int> CategoryIds { get; set; }
        public ProductStatus Status { get; set; }
    }

    public class ProductRequest : BasicProductRequest
    {
        public int StockQuantity { get; set; }
    }

    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal RewardPointsPercentage { get; set; }
        public decimal MaxRewardPoints { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus Status { get; set; }
    }

    public class ProductDetailResponse : ProductResponse
    {
        public List<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
    }

    public class UpdateProductStockRequest 
    {
        public int NewStockQuantity { get; set; }
    }
}
