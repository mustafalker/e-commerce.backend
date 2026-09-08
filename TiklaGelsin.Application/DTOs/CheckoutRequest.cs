namespace TiklaGelsin.Application.DTOs
{
    public class CheckoutRequest
    {
        public decimal TotalAmount { get; set; }
        public string PaymentType { get; set; } = string.Empty;
    }
}
