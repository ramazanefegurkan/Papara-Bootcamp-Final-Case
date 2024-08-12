using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public interface IOrderHandler
    {
        int Order { get; }
        Task Handle(OrderContext context);
    }
}
