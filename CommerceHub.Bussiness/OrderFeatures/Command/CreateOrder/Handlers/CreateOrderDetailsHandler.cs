using CommerceHub.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class CreateOrderDetailsHandler : IOrderHandler
    {
        public int Order => 4;
        public async Task Handle(OrderContext context)
        {
            var orderDetails = new List<OrderDetail>();

            foreach (var item in context.OrderRequest.Items)
            {
                var product = context.Products.First(p => p.Id == item.ProductId);

                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price,
                    ProductName = product.Name,
                };
                orderDetails.Add(orderDetail);
            }

            context.Order.OrderDetails = orderDetails;

            await Task.CompletedTask;
        }
    }
}
