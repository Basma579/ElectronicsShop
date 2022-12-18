using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.DTOs
{
     public class ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }

        public string  CategoryName { get; set; }

    }
}
