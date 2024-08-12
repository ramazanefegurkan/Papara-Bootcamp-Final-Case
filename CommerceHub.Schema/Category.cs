using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Schema
{
    public class CategoryRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
    }

    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }
    }

    public class CategoryDetailResponse : CategoryResponse
    {
        public List<ProductResponse> Products { get; set; } = new List<ProductResponse>();
    }
}
