using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.Interfaces
{
   public interface IProductService
    {
        List<ProductDTO> GetAllProduct(out int total , int CurrentPage , int MaxRow);
        DbOperationStatusEnum  CreateProduct(ProductDTO product   );

        DbOperationStatusEnum EditProduct(ProductDTO product);
        ProductDTO GetProductByID( int ProductID);
        List<ProductDTO> GetProductByCategoryID(out int total , int ? CategoryID);




    }
}
