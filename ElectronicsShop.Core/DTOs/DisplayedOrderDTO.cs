using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.DTOs
{
   public class DisplayedOrderDTO
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
