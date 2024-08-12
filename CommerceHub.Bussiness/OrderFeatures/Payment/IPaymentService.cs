using CommerceHub.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPayment(CreditCardInfo creditCardInfo, decimal amount);
    }
}
