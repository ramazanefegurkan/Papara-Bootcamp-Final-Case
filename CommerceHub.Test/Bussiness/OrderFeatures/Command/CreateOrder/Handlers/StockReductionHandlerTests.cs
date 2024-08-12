using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Repositories.GenericRepository;
using CommerceHub.Data.UnitOfWork;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class StockReductionHandlerTests
    {
        private readonly StockReductionHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Product>> _productRepositoryMock;
        public StockReductionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IGenericRepository<Product>>();
            _unitOfWorkMock.SetupGet(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
            _handler = new StockReductionHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReduceStockCorrectly_ForEachOrderDetail()
        {
            // Arrange
            var context = new OrderContext
            {
                Order = new Order
                {
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { ProductId = 1, Quantity = 2 },
                        new OrderDetail { ProductId = 2, Quantity = 3 }
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, StockQuantity = 10 },
                    new Product { Id = 2, StockQuantity = 15 }
                }
            };

            // Act
            await _handler.Handle(context);

            // Assert
            context.Products.First(x => x.Id == 1).StockQuantity.Should().Be(8); 
            context.Products.First(x => x.Id == 2).StockQuantity.Should().Be(12); 

            _unitOfWorkMock.Verify(uow => uow.ProductRepository.Update(It.IsAny<Product>()), Times.Exactly(2));
            _unitOfWorkMock.Verify(uow => uow.ProductRepository.Update(It.Is<Product>(p => p.Id == 1 && p.StockQuantity == 8)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.ProductRepository.Update(It.Is<Product>(p => p.Id == 2 && p.StockQuantity == 12)), Times.Once);
        }
    }
}

