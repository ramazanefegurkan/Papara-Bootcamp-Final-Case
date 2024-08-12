using CommerceHub.Bussiness.Exceptions;
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
    public class CheckProductStockHandlerTests
    {
        private readonly CheckProductStockHandler _handler;

        public CheckProductStockHandlerTests()
        {
            _handler = new CheckProductStockHandler();
        }

        [Fact]
        public async Task Handle_ShouldThrowInsufficientStockException_WhenProductStockIsInsufficient()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1, Quantity = 5 }
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, StockQuantity = 3 }
                }
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<InsufficientStockException>()
                .WithMessage("Product with ID 1 does not have enough stock. Available: 3, Requested: 5");
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException_WhenProductStockIsSufficient()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1, Quantity = 2 }
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, StockQuantity = 5 }
                }
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }
}

