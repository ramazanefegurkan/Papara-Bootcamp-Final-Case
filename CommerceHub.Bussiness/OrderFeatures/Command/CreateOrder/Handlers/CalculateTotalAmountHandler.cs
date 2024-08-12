using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class CalculateTotalAmountHandler : IOrderHandler
    {
        public int Order => 3;
        public async Task Handle(OrderContext context)
        {
            decimal totalAmount = 0;

            foreach (var item in context.OrderRequest.Items)
            {
                var product = context.Products.First(p => p.Id == item.ProductId);
                totalAmount += product.Price * item.Quantity;
            }

            context.Order.TotalAmount = totalAmount;
            context.RemainingAmount = totalAmount;

            await Task.CompletedTask;
        }
    }
}
