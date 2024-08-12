using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.Domain;
using CommerceHub.Schema;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class CreateOrderDetailsHandlerTests
    {
        private readonly CreateOrderDetailsHandler _handler;

        public CreateOrderDetailsHandlerTests()
        {
            _handler = new CreateOrderDetailsHandler();
        }

        [Fact]
        public async Task Handle_ShouldCreateOrderDetailsCorrectly()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1, Quantity = 2 },
                        new OrderItemRequest { ProductId = 2, Quantity = 3 }
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1", Price = 10m },
                    new Product { Id = 2, Name = "Product 2", Price = 20m }
                },
                Order = new Order()
            };

            // Act
            await _handler.Handle(context);

            // Assert
            context.Order.OrderDetails.Should().HaveCount(2);
            context.Order.OrderDetails.Should().ContainSingle(od =>
                od.ProductId == 1 && od.Quantity == 2 && od.Price == 10m && od.ProductName == "Product 1");
            context.Order.OrderDetails.Should().ContainSingle(od =>
                od.ProductId == 2 && od.Quantity == 3 && od.Price == 20m && od.ProductName == "Product 2");
        }
    }
}
