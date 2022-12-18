using ElectronicShope;
using ElectronicShope.DBModel.DBModel;
using ElectronicShope.Mapper;
using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using ElectronicsShop.Core.Interfaces;
using ElectronicsShop.Services.Common.Mapper;
using ElectronicsShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
 

        public AdminController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region Product
       
        public IActionResult AddProduct()
        {
            int total = 0;
            List<CategoryDTO> category = _categoryService.GetAllCategory(out total);
            ViewBag.CategoryName = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() });
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var insertedProduct = WebAutoMapper.Mapper.Map<ProductViewModel, ProductDTO>(product);
                _productService.CreateProduct(insertedProduct);

            }

            return RedirectToAction("ListProducts");
        }

        [HttpPost]
        public IActionResult ListProducts(ProductsListViewModel model)
        {
            int total = 0;

            List<CategoryDTO> category = _categoryService.GetAllCategory(out total);

            ViewBag.CategoryName = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() });

            List<ProductDTO> ProductList = new List<ProductDTO>();
            List<ProductViewModel> productViewModelsList = new List<ProductViewModel>();

            if (model.FilteredCategory >0)
            {
                ProductList = _productService.GetProductByCategoryID(out total, model.FilteredCategory);

            }
            else
            {
                ProductList = _productService.GetAllProduct(out total,1,1000);
            }
            if (ProductList != null)
            {
                foreach (ProductDTO productDTO in ProductList)
                {
                    ProductViewModel ProductVMList = WebAutoMapper.Mapper.Map<ProductDTO, ProductViewModel>(productDTO);
                    productViewModelsList.Add(ProductVMList);
                }
            }

            ProductsListViewModel productsListViewModel = new ProductsListViewModel();
            productsListViewModel.productViewModels = productViewModelsList;
            productsListViewModel.CategoryList = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();

    
            return View(productsListViewModel);
        }
        public IActionResult ListProducts()
        {
            int total = 0;

            List<CategoryDTO> category = _categoryService.GetAllCategory(out total);
            ViewBag.CategoryName = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() });



            List<ProductDTO> ProductList = new List<ProductDTO>();
            List<ProductViewModel> productViewModelsList = new List<ProductViewModel>();

             ProductList = _productService.GetAllProduct(out total,1,1000);
            
            if (ProductList != null)
            {
                foreach (ProductDTO productDTO in ProductList)
                {
                    ProductViewModel ProductVMList = WebAutoMapper.Mapper.Map<ProductDTO, ProductViewModel>(productDTO);
                    productViewModelsList.Add(ProductVMList);
                }
            }

            ProductsListViewModel productsListViewModel = new ProductsListViewModel();
            productsListViewModel.productViewModels = productViewModelsList;
            productsListViewModel.CategoryList = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();

            return View(productsListViewModel);
        }
        [HttpGet]
        public IActionResult EditProducts( int ID)
        {
            int total = 0; 
            List<CategoryDTO> category = _categoryService.GetAllCategory(out total);

          //  ViewBag.CategoryName = category.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() });

            var product = _productService.GetProductByID(ID);

            if (product != null)
            {
               var  ProductItem = WebAutoMapper.Mapper.Map<ProductDTO, ProductViewModel>(product);
                ViewBag.CategoryName = new SelectList(category, "ID", "Name",ProductItem.CategoryID);

                return View(ProductItem);
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditProducts(ProductViewModel productViewModel )
        {
            if (ModelState.IsValid)
            {
                var UpdatedProduct = WebAutoMapper.Mapper.Map<ProductViewModel, ProductDTO>(productViewModel);
                _productService.EditProduct(UpdatedProduct);
              return  RedirectToAction("ListProducts", "Admin");
            }

            return View();
        }


        #endregion


        #region Category
        public IActionResult AddCategory()
        {
            return View();
        }

         [HttpPost]
        public IActionResult AddCategory(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var insertedCategory = WebAutoMapper.Mapper.Map<CategoryViewModel, CategoryDTO>(categoryViewModel);
                _categoryService.CreateCategory(insertedCategory);

            }

            return RedirectToAction("ListCategory");
        }

        public IActionResult ListCategory()
        {
            int total = 0;
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            var categorylst = _categoryService.GetAllCategory(out total );
            if (categorylst != null)
            {
                foreach (CategoryDTO categoryDTO in categorylst)
                {
                    CategoryViewModel  categoryViewModel = WebAutoMapper.Mapper.Map<CategoryDTO, CategoryViewModel>(categoryDTO);
                    categoryViewModels.Add(categoryViewModel);
                }
            }
            return View(categoryViewModels);
        }

        #endregion
    }
}
