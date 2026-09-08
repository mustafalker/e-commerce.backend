using System;

namespace TiklaGelsin.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public CartItem(Guid id, Guid userId, Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new ArgumentException("Miktar en az 1 olmalıdır.");
            
            Id = id;
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new ArgumentException("Miktar en az 1 olmalıdır.");
            Quantity = newQuantity;
        }

        // EF Core için
        private CartItem() { }
    }
}
