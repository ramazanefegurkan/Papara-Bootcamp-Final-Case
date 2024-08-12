using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Messaging.Events
{
    public class OrderPlacedEvent
    {
        public string OrderNumber { get; set; }
        public string UserEmail { get; set; }
        public string UserFullName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
