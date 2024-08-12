using CommerceHub.Bussiness.Messaging.Events;
using CommerceHub.Bussiness.Notification;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Messaging.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly INotificationSender _notificationSender;

        public OrderPlacedConsumer(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var message = context.Message;
            await _notificationSender.SendNotificationAsync(message.UserFullName,message.UserEmail, "Sipariş Onayı", $"Siparişiniz başarıyla alındı. Sipariş Numarası: {message.OrderNumber}");
        }
    }
}
