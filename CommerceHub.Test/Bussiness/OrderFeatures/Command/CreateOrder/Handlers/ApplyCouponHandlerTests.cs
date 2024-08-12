using CommerceHub.Base;
using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class ApplyCouponHandlerTests
    {
        private readonly ApplyCouponHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISessionContext> _sessionContextMock;

        public ApplyCouponHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sessionContextMock = new Mock<ISessionContext>();
            _handler = new ApplyCouponHandler(_unitOfWorkMock.Object, _sessionContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCouponDoesNotExist()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CouponCode = "INVALID"
                }
            };

            _unitOfWorkMock.Setup(uow => uow.CouponRepository.FirstOrDefault(It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync((Coupon)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Coupon not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowCouponExpiredException_WhenCouponIsExpired()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CouponCode = "EXPIRED"
                }
            };

            var expiredCoupon = new Coupon
            {
                Code = "EXPIRED",
                ExpiryDate = DateTime.Now.AddDays(-1)
            };

            _unitOfWorkMock.Setup(uow => uow.CouponRepository.FirstOrDefault(It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(expiredCoupon);

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<CouponExpiredException>()
                .WithMessage($"Coupon expired on: {expiredCoupon.ExpiryDate.ToString("dd/MM/yyyy")}");
        }

        [Fact]
        public async Task Handle_ShouldThrowCouponAlreadyUsedException_WhenCouponAlreadyUsed()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CouponCode = "USED"
                }
            };

            var validCoupon = new Coupon
            {
                Id = 1,
                Code = "USED",
                ExpiryDate = DateTime.Now.AddDays(1)
            };

            _unitOfWorkMock.Setup(uow => uow.CouponRepository.FirstOrDefault(It.IsAny<Expression<Func<Coupon, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(validCoupon);

            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1});

            _unitOfWorkMock.Setup(uow => uow.OrderRepository.GetAll(It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Order> { new Order { CouponId = validCoupon.Id } });

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<CouponAlreadyUsedException>()
                .WithMessage("Coupon already used");
        }

        [Fact]
        public async Task Handle_ShouldApplyCouponCorrectly_WhenCouponIsValid()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CouponCode = "VALID"
                },
                Order = new Order(),
                RemainingAmount = 100
            };

            var validCoupon = new Coupon
            {
                Id = 1,
                Code = "VALID",
                Amount = 10,
                ExpiryDate = DateTime.Now.AddDays(1)
            };

            _unitOfWorkMock.Setup(uow => uow.CouponRepository.FirstOrDefault(It.IsAny<Expression<Func<Coupon, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(validCoupon);

            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1 });

            _unitOfWorkMock.Setup(uow => uow.OrderRepository.GetAll(It.IsAny<Expression<Func<Order, bool>>>(),
                   It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                   It.IsAny<string[]>()))
                .ReturnsAsync(new List<Order>());

            // Act
            await _handler.Handle(context);

            // Assert
            context.Order.CouponId.Should().Be(validCoupon.Id);
            context.Order.CouponAmount.Should().Be(validCoupon.Amount);
            context.RemainingAmount.Should().Be(90);
        }
    }
}
