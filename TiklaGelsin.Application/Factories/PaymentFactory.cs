using System;
using System.Collections.Generic;
using System.Linq;
using TiklaGelsin.Domain.Interfaces;

namespace TiklaGelsin.Application.Factories
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;

        public PaymentFactory(IEnumerable<IPaymentMethod> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        public IPaymentMethod CreatePaymentMethod(string paymentType)
        {
            var paymentMethod = _paymentMethods.FirstOrDefault(p => p.PaymentType.Equals(paymentType, StringComparison.OrdinalIgnoreCase));
            
            if (paymentMethod == null)
            {
                throw new ArgumentException($"Geçersiz ödeme türü: {paymentType}");
            }

            return paymentMethod;
        }
    }
}
