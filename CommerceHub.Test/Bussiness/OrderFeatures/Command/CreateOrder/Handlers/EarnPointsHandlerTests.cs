using CommerceHub.Base;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class EarnPointsHandlerTests
    {
        private readonly EarnPointsHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISessionContext> _sessionContextMock;

        public EarnPointsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sessionContextMock = new Mock<ISessionContext>();

            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1});

            _handler = new EarnPointsHandler(_unitOfWorkMock.Object, _sessionContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCalculateAndAddPointsCorrectly_WhenMaxLimitExceeded()
        {
            // Arrange
            var context = new OrderContext
            {
                Order = new Order
                {
                    TotalAmount = 250,
                    PaidAmount = 200,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { ProductId = 1, Price = 50, Quantity = 2 },//80
                        new OrderDetail { ProductId = 2, Price = 150, Quantity = 1 }//120
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, RewardPointsPercentage = 10, MaxRewardPoints = 5 },//8
                    new Product { Id = 2, RewardPointsPercentage = 20, MaxRewardPoints = 10 }//10
                }
            };

            var user = new User
            {
                Id = 1,
                WalletBalance = 10
            };

            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetById(user.Id)).ReturnsAsync(user);

            // Act
            await _handler.Handle(context);

            // Assert
            context.TotalPointsEarned.Should().Be(18);
            context.Order.EarnedPoints.Should().Be(18);
            user.WalletBalance.Should().Be(28);

            _unitOfWorkMock.Verify(uow => uow.UserRepository.Update(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCalculateAndAddPointsCorrectly_WhenMaxLimitNotExceeded()
        {
            // Arrange
            var context = new OrderContext
            {
                Order = new Order
                {
                    TotalAmount = 250,
                    PaidAmount = 200,
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail { ProductId = 1, Price = 50, Quantity = 2 },//80
                        new OrderDetail { ProductId = 2, Price = 150, Quantity = 1 }//120
                    }
                },
                Products = new List<Product>
                {
                    new Product { Id = 1, RewardPointsPercentage = 10, MaxRewardPoints = 5 },//8
                    new Product { Id = 2, RewardPointsPercentage = 20, MaxRewardPoints = 30 }//24
                }
            };

            var user = new User
            {
                Id = 1,
                WalletBalance = 10
            };

            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetById(user.Id)).ReturnsAsync(user);

            // Act
            await _handler.Handle(context);

            // Assert
            context.TotalPointsEarned.Should().Be(32);
            context.Order.EarnedPoints.Should().Be(32);
            user.WalletBalance.Should().Be(42);

            _unitOfWorkMock.Verify(uow => uow.UserRepository.Update(user), Times.Once);
        }
    }
}
