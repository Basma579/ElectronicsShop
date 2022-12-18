using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShope.DBModel.DBModel
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("OrderID")]
        public int OrderID { get; set; }
        public Order order { get; set; }

        [ForeignKey("ProductID")]
        [Required]
        public Product Product { get; set; }
        public int ProductID { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceAfterDiscount { get; set; }
    }
}
