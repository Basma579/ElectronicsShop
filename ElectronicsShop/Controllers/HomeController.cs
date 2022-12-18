using ElectronicShope;
using ElectronicShope.DBModel.DBModel;
using ElectronicShope.Mapper;
using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Interfaces;
using ElectronicsShop.Models;
using ElectronicsShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using ElectronicsShop.Core.Enums;
using System.Security.Claims;

namespace ElectronicsShop.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        public HomeController(UserManager<IdentityUser> userManager, IProductService productService, IOrderService orderService)
        {

            this.userManager = userManager;
            _productService = productService;
            _orderService = orderService;

        }


        public IActionResult Index()
        {
            SelectedProductsViewModel selectedProductsViewModel = GetCurrentPage(1);

            return View(selectedProductsViewModel);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Index(SelectedProductsViewModel model)
        {
            if (model.SelecedIds != null)
            {
                var key = "data";
                var str = JsonConvert.SerializeObject(model);
                HttpContext.Session.SetString(key, str);
            }
            if (string.IsNullOrEmpty(model.SelecedIds) && !User.Identity.IsAuthenticated)
            {
                return View("EmptyCard");
            }
            var CashedData = HttpContext.Session.GetString("data");
            if (CashedData == null)
            {
                model = CalculateDiscount(model);
                var key = "data";
                var str = JsonConvert.SerializeObject(model);
                HttpContext.Session.SetString(key, str);
            }
            else
            {
                SelectedProductsViewModel ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(CashedData);
                if (ListItems.SelecedIds != model.SelecedIds)
                {
                    model = CalculateDiscount(model);
                    var key = "data";
                    var str = JsonConvert.SerializeObject(model);
                    HttpContext.Session.SetString(key, str);

                }
            }



            if (!string.IsNullOrEmpty(CashedData))
            {

                SelectedProductsViewModel ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(CashedData);

                var items = ListItems.Items;
                foreach (var item in items.ToList())
                {
                    if (!ListItems.SelecedIds.Contains(item.ID.ToString()))
                    {
                        items.Remove(item);
                    }
                }

                model.Items = items;
                model = CalculateDiscount(model);

                var strVal = JsonConvert.SerializeObject(model);
                HttpContext.Session.SetString("data", strVal);


                //if (User.Identity.IsAuthenticated)
                //{

                return RedirectToAction("ViewCartItems");

                //}
                //else
                //{

                //    return RedirectToAction("Login", "Account");
                //}
            }
            else
            {
                return View("EmptyCard");
            }


        }
        public void GETItems()
        {
            var str = HttpContext.Session.GetString("data");
            var ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(str);

        }


        public ActionResult ViewCartItems()
        {
            var str = HttpContext.Session.GetString("data");
            if (!string.IsNullOrEmpty(str))
            {

                var ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(str);
                ListItems = CalculateDiscount(ListItems);
                return View("CartItems", ListItems.Items);
            }
            else
            {
                return View("EmptyCard");
            }
        }

        [HttpPost]
        public IActionResult ChangeQuantity(int ItemID, bool IsIncrease)
        {

            var str = HttpContext.Session.GetString("data");
            var ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(str);
            var Products = ListItems.Items;

            if (ItemID > 0)
            {
                foreach (var prod in Products)
                {
                    if (prod.ID == ItemID)
                    {
                        if (IsIncrease)
                        {
                            prod.Quantity++;
                        }
                        else
                        {
                            prod.Quantity--;
                        }
                    }
                }
            }


            var key = "data";
            HttpContext.Session.Remove(key);
            SelectedProductsViewModel selectedProductsViewModel = new SelectedProductsViewModel();
            selectedProductsViewModel.Items = Products;
            selectedProductsViewModel = CalculateDiscount(selectedProductsViewModel);

            var CashedData = JsonConvert.SerializeObject(selectedProductsViewModel);
            HttpContext.Session.SetString(key, CashedData);
            return View("CartItems", Products);

        }


        public SelectedProductsViewModel CalculateDiscount(SelectedProductsViewModel model)
        {

            var items = model.Items;
            foreach (var item in items)
            {
                if (item.Discount != null && item.Discount > 0 && item.Quantity >= 2)
                {
                    var DiscountValue = ((decimal)((item.Discount * item.Quantity* item.Price) / (100)));
                    item.DiscountValue = DiscountValue;
                    item.PriceAfterDiscount = ((item.Price * item.Quantity) - DiscountValue);
                }
                else
                {
                    item.PriceAfterDiscount = (item.Price * item.Quantity);
                    item.DiscountValue = 0;
                }

            }

            model.Items = items;
            return model;
        }



        [HttpPost]
        public IActionResult RemoveCartItem(int Product_ID)
        {

            var str = HttpContext.Session.GetString("data");
            var ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(str);
            var Products = ListItems.Items;
            foreach (var item in Products.ToList())
            {
                if (item.ID == Product_ID)
                {
                    Products.Remove(item);
                }
            }
            SelectedProductsViewModel selectedProductsViewModel = new SelectedProductsViewModel();
            selectedProductsViewModel.Items = Products;

            var CashedData = JsonConvert.SerializeObject(selectedProductsViewModel);
            HttpContext.Session.SetString("data", CashedData);

            return View("CartItems", Products);
        }


        public IActionResult AddOrder()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<OrderDetailsDTO> OrderDetailsLst = new List<OrderDetailsDTO>();

            OrderDTO orderDTO = new OrderDTO();
            orderDTO.UserID = userId;

            var str = HttpContext.Session.GetString("data");
            var ListItems = JsonConvert.DeserializeObject<SelectedProductsViewModel>(str);
            var Products = ListItems.Items;


            foreach (var item in Products)
            {
                OrderDetailsDTO orderDetails = new OrderDetailsDTO();
                orderDetails.ProductID = item.ID;
                orderDetails.Price = item.Price;
                orderDetails.Quantity = item.Quantity;
                orderDetails.Discount = (decimal)item.DiscountValue;
                orderDetails.PriceAfterDiscount = (decimal)item.PriceAfterDiscount;

                orderDTO.Total += (decimal)item.PriceAfterDiscount;
                orderDTO.TotalItemsCount += item.Quantity;
                orderDTO.TotalDiscount += (decimal)item.DiscountValue;


                OrderDetailsLst.Add(orderDetails);

            }

            orderDTO.OrderDetails = OrderDetailsLst;

            DbOperationStatusEnum dbOperationStatusEnum = _orderService.CreateOrder(orderDTO);
            if (dbOperationStatusEnum == DbOperationStatusEnum.Success)
            {
                HttpContext.Session.Remove("data");

            }
            return RedirectToAction("ViewMyOrders");
        }

        [HttpPost]
        public IActionResult ConfirmOrder()
        {

            if (User.Identity.IsAuthenticated)
            {
                AddOrder();
                return RedirectToAction("ViewMyOrders");

            }

            else
            {
                return RedirectToAction("Login", "Account");
            }

        }


        public ActionResult ViewMyOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                int total = 0;
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                List<OrderViewModel> orderViewModels = new List<OrderViewModel>();
                var orders = _orderService.GetUserOrders(out total, userId);
                foreach (DisplayedOrderDTO displayedOrderDTO in orders)
                {
                    OrderViewModel orderViewModel = WebAutoMapper.Mapper.Map<DisplayedOrderDTO, OrderViewModel>(displayedOrderDTO);
                    orderViewModels.Add(orderViewModel);

                }
                return View("MyOrders", orderViewModels);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }




        [HttpPost]
        public IActionResult GetNexPage(int currentPageIndex)
        {
            return View("Index", GetCurrentPage(currentPageIndex));
        }

        private SelectedProductsViewModel GetCurrentPage(int currentPage)
        {


            int total = 0;
            int maxRows = 5;
            SelectedProductsViewModel selectedProductsViewModels = new SelectedProductsViewModel();
            List<ProductDTO> ProductList = new List<ProductDTO>();
            List<ProductViewModel> productViewModelsList = new List<ProductViewModel>();

            ProductList = _productService.GetAllProduct(out total,currentPage,maxRows);

            if (ProductList != null)
            {
                foreach (ProductDTO productDTO in ProductList)
                {
                    ProductViewModel ProductVMList = WebAutoMapper.Mapper.Map<ProductDTO, ProductViewModel>(productDTO);
                    productViewModelsList.Add(ProductVMList);
                }
            }
            selectedProductsViewModels.Items = productViewModelsList;

            double pageCount = (double)((decimal)total / Convert.ToDecimal(maxRows));
            selectedProductsViewModels.PageCount = (int)Math.Ceiling(pageCount);

            selectedProductsViewModels.CurrentPageIndex = currentPage;

            return selectedProductsViewModels;
        }
    }
}
