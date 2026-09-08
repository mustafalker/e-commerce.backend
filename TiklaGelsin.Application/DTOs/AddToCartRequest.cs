using System;

namespace TiklaGelsin.Application.DTOs
{
    public class AddToCartRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
