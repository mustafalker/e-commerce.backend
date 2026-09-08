using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiklaGelsin.Infrastructure.Persistence
{
    [Table("CartItems")]
    public class CartItemEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [ForeignKey("UserId")]
        public UserEntity User { get; set; } = null!;
        
        [ForeignKey("ProductId")]
        public ProductEntity Product { get; set; } = null!;
    }
}
