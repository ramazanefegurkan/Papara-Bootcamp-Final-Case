using CommerceHub.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Payment
{
    public class FakePaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPayment(CreditCardInfo creditCardInfo, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(creditCardInfo.CardNumber) ||
                string.IsNullOrWhiteSpace(creditCardInfo.ExpiryDate) ||
                string.IsNullOrWhiteSpace(creditCardInfo.CVV))
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid credit card information."
                };
            }

            if (creditCardInfo.CVV.Length != 3)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid CVV."
                };
            }

            if (creditCardInfo.CardNumber.StartsWith("4") || creditCardInfo.CardNumber.StartsWith("5"))
            {
                return new PaymentResult
                {
                    IsSuccess = true,
                    ErrorMessage = null
                };
            }

            return new PaymentResult
            {
                IsSuccess = false,
                ErrorMessage = "Invalid credit card number."
            };
        }
    }
}
