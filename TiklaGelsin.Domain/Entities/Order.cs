using System;
using TiklaGelsin.Domain.Exceptions;

namespace TiklaGelsin.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsPaid { get; private set; }

        public Order(Guid id, string userId, decimal totalAmount)
        {
            if (totalAmount <= 0)
                throw new DomainException("Total amount must be greater than zero.");

            Id = id;
            UserId = userId;
            TotalAmount = totalAmount;
            CreatedAt = DateTime.UtcNow;
            IsPaid = false;
        }

        public void MarkAsPaid()
        {
            if (IsPaid)
                throw new DomainException("Order is already paid.");

            IsPaid = true;
        }
    }
}
