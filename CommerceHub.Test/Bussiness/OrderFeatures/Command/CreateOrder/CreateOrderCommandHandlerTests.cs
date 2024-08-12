using AutoMapper;
using CommerceHub.Base;
using CommerceHub.Bussiness.Messaging;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Schema;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Bussiness.OrderFeatures.Payment;
using CommerceHub.Data.Domain;
using CommerceHub.Bussiness.Messaging.Events;
using System.Linq.Expressions;
using CommerceHub.Data.Repositories.GenericRepository;
using System.Collections;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder
{
    public class CreateOrderCommandHandlerTests
    {
        public static IEnumerable<object[]> GetPaymentTestCases()
        {
            yield return new object[] { new OrderRequest { CouponCode = "VALID100", Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 100m, 0m, 0m, 0m, 2 }; // Sadece kupon
            yield return new object[] { new OrderRequest { CouponCode = "VALID", PointsToUse = 30, Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 40m, 30m, 30m, 3m, 1 }; // Kupon ve puan
            yield return new object[] { new OrderRequest { CouponCode = "VALID", CreditCard = new CreditCardInfo { CardNumber = "4111111111111111", ExpiryDate = "12/25", CVV = "123" }, Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 40m, 0m, 60m, 6m, 1 }; // Kupon ve kredi kartı
            yield return new object[] { new OrderRequest { PointsToUse = 30, Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 0m, 30m, 70m, 7m, null }; // Sadece puan
            yield return new object[] { new OrderRequest { CreditCard = new CreditCardInfo { CardNumber = "4111111111111111", ExpiryDate = "12/25", CVV = "123" }, Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 0m, 0m, 100m, 10m, null }; // Sadece kredi kartı
            yield return new object[] { new OrderRequest { PointsToUse = 30, CreditCard = new CreditCardInfo { CardNumber = "4111111111111111", ExpiryDate = "12/25", CVV = "123" }, Items = new List<OrderItemRequest> { new OrderItemRequest { ProductId = 1, Quantity = 2 } } }, 100m, 0m, 30m, 70m, 7m, null }; // Puan ve kredi kartı
        }

        private readonly CreateOrderCommandHandler _handler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISessionContext> _sessionContextMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly List<IOrderHandler> _orderHandlerMocks;

        private readonly Mock<IGenericRepository<Product>> _productRepositoryMock;
        private readonly Mock<IGenericRepository<Coupon>> _couponRepositoryMock;
        private readonly Mock<IGenericRepository<User>> _userRepositoryMock;
        private readonly Mock<IGenericRepository<Order>> _orderRepositoryMock;
        public CreateOrderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IGenericRepository<Product>>();
            _couponRepositoryMock = new Mock<IGenericRepository<Coupon>>();
            _userRepositoryMock = new Mock<IGenericRepository<User>>();
            _orderRepositoryMock = new Mock<IGenericRepository<Order>>();
            _unitOfWorkMock.SetupGet(u => u.ProductRepository).Returns(_productRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.CouponRepository).Returns(_couponRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.UserRepository).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.SetupGet(u => u.OrderRepository).Returns(_orderRepositoryMock.Object);

            _mapperMock = new Mock<IMapper>();
            _sessionContextMock = new Mock<ISessionContext>();
            _sessionContextMock.Setup(s => s.Session).Returns(new Session() { UserId = 1, Email = "test@test.com", FullName = "Test Test" });
            _messagePublisherMock = new Mock<IMessagePublisher>();

            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(ps => ps.ProcessPayment(It.IsAny<CreditCardInfo>(), It.IsAny<decimal>()))
                .ReturnsAsync(new PaymentResult { IsSuccess = true });

            _orderHandlerMocks = new List<IOrderHandler>
            {
               new FetchProductsHandler(_unitOfWorkMock.Object),
               new CheckProductStockHandler(),
               new CalculateTotalAmountHandler(),
               new CreateOrderDetailsHandler(),
               new ApplyCouponHandler(_unitOfWorkMock.Object, _sessionContextMock.Object),
               new ApplyPointsHandler(_unitOfWorkMock.Object, _sessionContextMock.Object),
               new ProcessPaymentHandler(paymentServiceMock.Object),
               new StockReductionHandler(_unitOfWorkMock.Object),
               new EarnPointsHandler(_unitOfWorkMock.Object, _sessionContextMock.Object)
            };

            _handler = new CreateOrderCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _sessionContextMock.Object,
                _messagePublisherMock.Object,
                _orderHandlerMocks
            );
        }

        [Theory]
        [MemberData(nameof(GetPaymentTestCases))]
        public async Task Handle_ShouldSetOrderEntityFieldsCorrectly_ForVariousPaymentCombinations(OrderRequest request, decimal expectedTotalAmount, decimal expectedCouponAmount, decimal expectedUsedPoints, decimal expectedPaidAmount, decimal expectedEarnedPoints, int? expectedCouponId)
        {
            var command = new CreateOrderCommand(request);

            var coupon = new Coupon
            {
                Id = 1,
                Code = "VALID",
                Amount = 40, 
                ExpiryDate = DateTime.Now.AddDays(1)
            };

            var couponFull = new Coupon
            {
                Id = 2,
                Code = "VALID100",
                Amount = 100,
                ExpiryDate = DateTime.Now.AddDays(1)
            };

            var product = new Product
            {
                Id = 1,
                Price = 50, 
                StockQuantity = 10,
                RewardPointsPercentage = 10, 
                MaxRewardPoints = 5
            };

            var user = new User
            {
                Id = 1,
                WalletBalance = 50 
            };

            _unitOfWorkMock.Setup(uow => uow.CouponRepository.FirstOrDefault(
                It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Coupon, bool>> expr, string[] includeProperties) =>
                {
                    var compiledExpr = expr.Compile();
                    if (compiledExpr(coupon))
                        return coupon;
                    if (compiledExpr(couponFull))
                        return couponFull;

                    return null;
                });

            _unitOfWorkMock.Setup(uow => uow.OrderRepository.GetAll(
                    It.IsAny<Expression<Func<Order, bool>>>(),
                    It.IsAny<Func<IQueryable<Order>, IOrderedQueryable<Order>>>(),
                    It.IsAny<string[]>())).ReturnsAsync(new List<Order>());

            _unitOfWorkMock.Setup(uow => uow.ProductRepository.GetById(It.IsAny<long>()))
                .ReturnsAsync(product);

            _productRepositoryMock.Setup(repo => repo.GetAll(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<string[]>()))
                .ReturnsAsync(new List<Product>
                {
                    product
                });

            _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<long>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetById(It.IsAny<long>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(uow => uow.OrderRepository.Insert(It.IsAny<Order>()));
            _unitOfWorkMock.Setup(uow => uow.CompleteWithTransaction()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Order, OrderResponse>(It.IsAny<Order>())).Returns(new OrderResponse());

            Order capturedOrder = null;
            _unitOfWorkMock.Setup(uow => uow.OrderRepository.Insert(It.IsAny<Order>()))
                .Callback<Order>(o => capturedOrder = o);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Order created successfully.");

            // Doğru değerlerin Order entity'sine ayarlandığını kontrol et
            capturedOrder.Should().NotBeNull();
            capturedOrder.TotalAmount.Should().Be(expectedTotalAmount);
            capturedOrder.CouponAmount.Should().Be(expectedCouponAmount);
            capturedOrder.UsedPoints.Should().Be(expectedUsedPoints);
            capturedOrder.PaidAmount.Should().Be(expectedPaidAmount);
            capturedOrder.EarnedPoints.Should().Be(expectedEarnedPoints);
            capturedOrder.CouponId.Should().Be(expectedCouponId);

            capturedOrder.TotalAmount.Should().Be(100); 

            _messagePublisherMock.Verify(mp => mp.PublishOrderPlacedEventAsync(It.IsAny<OrderPlacedEvent>()), Times.Once);
        }

        [Fact]
        public async Task GenerateOrderNumberAsync_ShouldGenerateUniqueOrderNumber()
        {
            // Arrange
            _unitOfWorkMock.Setup(uow => uow.OrderRepository.FirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<System.Func<Order, bool>>>()))
                .ReturnsAsync((Order)null);

            // Act
            var orderNumber = await (Task<string>)_handler.GetType()
                .GetMethod("GenerateOrderNumberAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_handler, null);

            // Assert
            orderNumber.Should().NotBeNull();
            orderNumber.Should().StartWith("SIP");
        }
    }
}

