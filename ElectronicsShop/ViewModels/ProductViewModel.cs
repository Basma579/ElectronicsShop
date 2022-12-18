using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShope
{
   public class ProductViewModel
    {
       
        public int ID { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public decimal Price { get; set; }

        public decimal? DiscountValue { get; set; }

        [Required(ErrorMessage = "Required")] 
        public int Quantity { get; set; }
    
        public decimal? Discount { get; set; }

        public decimal? PriceAfterDiscount { get; set; }

    }
}
