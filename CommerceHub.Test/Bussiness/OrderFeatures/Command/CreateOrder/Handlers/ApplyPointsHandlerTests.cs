using CommerceHub.Base;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.Domain;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
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
    public class ApplyPointsHandlerTests
    {
        private readonly ApplyPointsHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISessionContext> _sessionContextMock;
        public ApplyPointsHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sessionContextMock = new Mock<ISessionContext>();

            _handler = new ApplyPointsHandler(_unitOfWorkMock.Object, _sessionContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowInsufficientPointsException_WhenUserDoesNotHaveEnoughPoints()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    PointsToUse = 100
                },
                RemainingAmount = 200
            };

            var user = new User
            {
                Id = 1,
                WalletBalance = 50 // Insufficient points
            };

            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1 });
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetById(user.Id)).ReturnsAsync(user);

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<InsufficientPointsException>()
                .WithMessage($"User does not have enough points. Available: {user.WalletBalance}, Requested: {context.OrderRequest.PointsToUse}");
        }

        [Fact]
        public async Task Handle_ShouldApplyPointsCorrectly_WhenUserHasEnoughPoints()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    PointsToUse = 40
                },
                RemainingAmount = 100,
                Order = new Order()
            };

            var user = new User
            {
                Id = 1,
                WalletBalance = 100 // Sufficient points
            };

            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1 });
            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetById(user.Id)).ReturnsAsync(user);

            // Act
            await _handler.Handle(context);

            // Assert
            context.Order.UsedPoints.Should().Be(40);
            context.RemainingAmount.Should().Be(60); 
            user.WalletBalance.Should().Be(60); 

            _unitOfWorkMock.Verify(uow => uow.UserRepository.Update(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldDoNothing_WhenRemainingAmountIsZeroOrLess()
        {
            // Arrange
            var context = new OrderContext
            {
                RemainingAmount = 0
            };

            // Act
            await _handler.Handle(context);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.UserRepository.Update(It.IsAny<User>()), Times.Never);
        }
    }
}
