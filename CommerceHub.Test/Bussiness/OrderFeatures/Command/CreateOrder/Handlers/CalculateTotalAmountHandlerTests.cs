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
    public class CalculateTotalAmountHandlerTests
    {
        private readonly CalculateTotalAmountHandler _handler;

        public CalculateTotalAmountHandlerTests()
        {
            _handler = new CalculateTotalAmountHandler();
        }

        [Fact]
        public async Task Handle_ShouldCalculateTotalAmountCorrectly()
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
                    new Product { Id = 1, Price = 10 },
                    new Product { Id = 2, Price = 20 }
                },
                Order = new Order()
            };

            // Act
            await _handler.Handle(context);

            // Assert
            context.Order.TotalAmount.Should().Be(80);
            context.RemainingAmount.Should().Be(80);
        }
    }
}
