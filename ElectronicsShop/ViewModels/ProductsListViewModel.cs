using ElectronicsShop.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ElectronicsShop
{
    public class ProductsListViewModel
    {
      public  List<ElectronicShope.ProductViewModel> productViewModels { set; get; }
        public List< SelectListItem>  CategoryList { set; get; }

        public int FilteredCategory { get; set; }

    }
}
