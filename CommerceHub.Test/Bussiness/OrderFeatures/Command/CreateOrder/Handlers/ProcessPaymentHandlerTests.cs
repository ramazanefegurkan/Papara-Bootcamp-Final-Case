using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Bussiness.OrderFeatures.Payment;
using CommerceHub.Data.Domain;
using CommerceHub.Schema;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Test.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class ProcessPaymentHandlerTests
    {
        private readonly ProcessPaymentHandler _handler;
        private readonly Mock<IPaymentService> _paymentServiceMock;

        public ProcessPaymentHandlerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _handler = new ProcessPaymentHandler(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowPaymentFailedException_WhenPaymentFails()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CreditCard = new CreditCardInfo { CardNumber = "4111111111111111", ExpiryDate = "12/25", CVV = "123" }
                },
                RemainingAmount = 100
            };

            _paymentServiceMock.Setup(ps => ps.ProcessPayment(It.IsAny<CreditCardInfo>(), It.IsAny<decimal>()))
                .ReturnsAsync(new PaymentResult { IsSuccess = false });

            // Act
            Func<Task> act = async () => await _handler.Handle(context);

            // Assert
            await act.Should().ThrowAsync<PaymentFailedException>()
                .WithMessage("Payment failed. Please check credit card information.");
        }

        [Fact]
        public async Task Handle_ShouldSetPaidAmount_WhenPaymentIsSuccessful()
        {
            // Arrange
            var context = new OrderContext
            {
                OrderRequest = new OrderRequest
                {
                    CreditCard = new CreditCardInfo { CardNumber = "4111111111111111", ExpiryDate = "12/25", CVV = "123" }
                },
                RemainingAmount = 100,
                Order = new Order()
            };

            _paymentServiceMock.Setup(ps => ps.ProcessPayment(It.IsAny<CreditCardInfo>(), It.IsAny<decimal>()))
                .ReturnsAsync(new PaymentResult { IsSuccess = true });

            // Act
            await _handler.Handle(context);

            // Assert
            context.Order.PaidAmount.Should().Be(100);
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
            _paymentServiceMock.Verify(ps => ps.ProcessPayment(It.IsAny<CreditCardInfo>(), It.IsAny<decimal>()), Times.Never);
        }
    }
}
