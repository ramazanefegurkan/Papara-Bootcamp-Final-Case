using CommerceHub.Bussiness.Messaging.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishOrderPlacedEventAsync(OrderPlacedEvent orderPlacedEvent)
        {
            await _publishEndpoint.Publish(orderPlacedEvent);
        }
    }
}
