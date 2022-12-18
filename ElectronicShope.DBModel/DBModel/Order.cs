using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShope.DBModel.DBModel
{
   public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(128), ForeignKey("UserId")]
        public string UserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        [Required]
        public int TotalItemsCount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalDiscount { get; set; }


        public ICollection<OrderDetails> OrderDetails { set; get; }  


    }
}
