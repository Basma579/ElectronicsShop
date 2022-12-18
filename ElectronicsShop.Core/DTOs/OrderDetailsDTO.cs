using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.DTOs
{
   public  class OrderDetailsDTO
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
    }
}
