using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.Interfaces
{
   public interface ICategoryService
    {
        List<CategoryDTO> GetAllCategory(out int total);
        DbOperationStatusEnum CreateCategory(CategoryDTO category);
    }
}
