using System;

namespace TiklaGelsin.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Product(Guid id, string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Ürün adı boş olamaz.");
            if (price < 0) throw new ArgumentException("Fiyat sıfırdan küçük olamaz.");

            Id = id;
            Name = name;
            Price = price;
        }

        // EF Core için private constructor
        private Product() { Name = string.Empty; }
    }
}
