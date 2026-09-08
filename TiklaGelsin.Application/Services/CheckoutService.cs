using System;
using System.Threading.Tasks;
using TiklaGelsin.Application.DTOs;
using TiklaGelsin.Application.Factories;
using TiklaGelsin.Application.Interfaces;
using TiklaGelsin.Domain.Entities;

namespace TiklaGelsin.Application.Services
{
    public class CheckoutService
    {
        private readonly IPaymentFactory _paymentFactory;
        private readonly IOrderRepository _orderRepository;

        public CheckoutService(IPaymentFactory paymentFactory, IOrderRepository orderRepository)
        {
            _paymentFactory = paymentFactory;
            _orderRepository = orderRepository;
        }

        public async Task<(bool Success, Guid OrderId, string ErrorMessage)> ProcessCheckoutAsync(CheckoutRequest request, string userId)
        {
            try
            {
                var order = new Order(Guid.NewGuid(), userId, request.TotalAmount);
                var paymentMethod = _paymentFactory.CreatePaymentMethod(request.PaymentType);

                bool isSuccess = await paymentMethod.ProcessPaymentAsync(order);

                if (isSuccess)
                {
                    await _orderRepository.AddAsync(order);
                    return (true, order.Id, string.Empty);
                }

                return (false, Guid.Empty, "Ödeme işlemi başarısız oldu.");
            }
            catch (Exception ex)
            {
                return (false, Guid.Empty, ex.Message);
            }
        }
    }
}
