using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Schema.Report
{
    public class CriticalStockLevelResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockLevel { get; set; }
        public int CriticalStockLevel { get; set; }
    }
}
