using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class CheckProductStockHandler : IOrderHandler
    {
        public int Order => 2;

        public async Task Handle(OrderContext context)
        {
            foreach (var item in context.OrderRequest.Items)
            {
                var product = context.Products.First(x => x.Id == item.ProductId);

                if (item.Quantity > product.StockQuantity)
                {
                    throw new InsufficientStockException($"Product with ID {item.ProductId} does not have enough stock. Available: {product.StockQuantity}, Requested: {item.Quantity}");
                }
            }

            await Task.CompletedTask;
        }
    }
}
