using FluentAssertions;
using Moq;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Schema;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Repositories.GenericRepository;
using System.Linq.Expressions;
using static Dapper.SqlMapper;
using CommerceHub.Bussiness.Exceptions;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class FetchProductsHandlerTests
    {
        private readonly FetchProductsHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGenericRepository<Product>> _productRepositoryMock;

        public FetchProductsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IGenericRepository<Product>>();
            _unitOfWorkMock.SetupGet(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
            _handler = new FetchProductsHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAnyProductIsNotFound()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1 },
                        new OrderItemRequest { ProductId = 2 }
                    }
                }
            };

            _productRepositoryMock.Setup(repo => repo.GetAll(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1" }
                });

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Products not found: 2");
        }

        [Fact]
        public async Task Handle_ShouldSetContextProducts_WhenAllProductsAreFound()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1 },
                        new OrderItemRequest { ProductId = 2 }
                    }
                }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _productRepositoryMock.Setup(repo => repo.GetAll(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(products);

            // Act
            await _handler.Handle(context);

            // Assert
            context.Products.Should().BeEquivalentTo(products);
        }
    }

    
}
