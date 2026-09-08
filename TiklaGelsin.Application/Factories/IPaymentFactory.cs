using TiklaGelsin.Domain.Interfaces;

namespace TiklaGelsin.Application.Factories
{
    public interface IPaymentFactory
    {
        IPaymentMethod CreatePaymentMethod(string paymentType);
    }
}
