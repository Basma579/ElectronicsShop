using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.DTOs
{
   public class OrderDTO
    {
        public int ID { get; set; }
        public string UserID { get; set; }

        public decimal Total { get; set; }
        public int TotalItemsCount { get; set; }
        
        public decimal? TotalDiscount { get; set; }
       public List<OrderDetailsDTO> OrderDetails { set; get; }

    }
}
