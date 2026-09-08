using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiklaGelsin.Infrastructure.Persistence
{
    [Table("OrderItems")]
    public class OrderItemEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [ForeignKey("OrderId")]
        public OrderEntity Order { get; set; } = null!;
        
        [ForeignKey("ProductId")]
        public ProductEntity Product { get; set; } = null!;
    }
}
