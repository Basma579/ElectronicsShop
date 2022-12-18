using ElectronicShope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.ViewModels
{
    public class SelectedProductsViewModel
    {
        public string SelecedIds { get; set; }
        public List<ProductViewModel> Items { get; set; }
        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }
    }
}
