using CommerceHub.Bussiness.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishOrderPlacedEventAsync(OrderPlacedEvent orderPlacedEvent);
    }
}
