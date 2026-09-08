using System;
using System.Linq;
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
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;

        public CheckoutService(
            IPaymentFactory paymentFactory, 
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IUserRepository userRepository)
        {
            _paymentFactory = paymentFactory;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        public async Task<(bool Success, Guid OrderId, string ErrorMessage)> ProcessCheckoutAsync(CheckoutRequest request, string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null) return (false, Guid.Empty, "Kullanıcı bulunamadı.");

                var cartItems = (await _cartRepository.GetCartByUserIdAsync(user.Id)).ToList();
                if (!cartItems.Any()) return (false, Guid.Empty, "Sepetiniz boş.");

                var totalAmount = cartItems.Sum(c => c.Quantity * c.UnitPrice);

                var order = new Order(Guid.NewGuid(), user.Id.ToString(), totalAmount);
                var paymentMethod = _paymentFactory.CreatePaymentMethod(request.PaymentType);

                bool isSuccess = await paymentMethod.ProcessPaymentAsync(order);

                if (isSuccess)
                {
                    await _orderRepository.AddAsync(order);
                    
                    // Sepeti temizle
                    await _cartRepository.ClearCartAsync(user.Id);

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
