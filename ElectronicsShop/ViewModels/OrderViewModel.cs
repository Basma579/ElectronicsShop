using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ElectronicShope
{
   public class OrderViewModel
    {
       
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        public decimal? DiscountValue { get; set; }

        public int Quantity { get; set; }
    
        public decimal? Discount { get; set; }

        public decimal? PriceAfterDiscount { get; set; }

        public decimal? Total { get; set; }

    }
}
