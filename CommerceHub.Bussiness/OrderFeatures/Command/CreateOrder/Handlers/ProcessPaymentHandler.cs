using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Bussiness.OrderFeatures.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class ProcessPaymentHandler : IOrderHandler
    {
        private readonly IPaymentService _paymentService;
        public int Order => 7;

        public ProcessPaymentHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(OrderContext context)
        {
            if (context.RemainingAmount <= 0)
                return;

            var paymentResult = await _paymentService.ProcessPayment(context.OrderRequest.CreditCard, context.RemainingAmount);
            if (!paymentResult.IsSuccess)
            {
                throw new PaymentFailedException("Payment failed. Please check credit card information.");
            }

            context.Order.PaidAmount = context.RemainingAmount;

            await Task.CompletedTask;
        }
    }
}
