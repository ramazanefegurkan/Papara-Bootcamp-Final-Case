using CommerceHub.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Tags { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new HashSet<CategoryProduct>();
    }
}
