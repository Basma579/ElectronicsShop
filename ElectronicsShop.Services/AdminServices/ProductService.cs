using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using ElectronicsShop.Core.Interfaces;
using ElectronicsShop.Repository.Interfaces;
using ElectronicsShop.Services.Common.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace ElectronicsShop.Services.AdminServices
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;

      public  ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DbOperationStatusEnum CreateProduct(ProductDTO product)
        {
            Product Insertedproduct = BusinessAutoMapper.Mapper.Map<ProductDTO, Product>(product);

            _unitOfWork.GetRepository<Product>().Insert(Insertedproduct);
            _unitOfWork.Commit();
            return DbOperationStatusEnum.Success;
        }

        public DbOperationStatusEnum EditProduct(ProductDTO product)
        {
            var query = from item in _unitOfWork.GetRepository<Product>().GetQueryable().Where(x => x.ID == product.ID)
                        select item;
            if (query.Any())
            {
                var item = query.First();
                item.Name = product.Name;
                item.Price = product.Price;
                item.Quantity = product.Quantity;
                item.Description = product.Description;
                item.Discount = product.Discount;
                item.CategoryID = product.CategoryID;

                _unitOfWork.GetRepository<Product>().Update(item);
                _unitOfWork.Commit();
                return DbOperationStatusEnum.Updated;
            }
            else {
                return DbOperationStatusEnum.ItemNotExist;

            }
        }

        public List<ProductDTO> GetAllProduct(out int total, int CurrentPage, int MaxRow)
        {
            var query = from product in _unitOfWork.GetRepository<Product>().GetQueryable()
                        join category in _unitOfWork.GetRepository<Category>().GetQueryable()
                        on new { cid= product.CategoryID } equals new {cid= category.ID }

                        select new ProductDTO
                        {
                            ID = product.ID,
                            Name = product.Name,
                            CategoryID=product.CategoryID ,
                            CategoryName = category.Name ,
                            Description = product.Description ,
                            Price=product.Price ,
                            Discount = (decimal)product.Discount , 
                            Quantity=product.Quantity
                        };
            total = query.Count();
            query = query.OrderBy(p => p.ID)
                        .Skip((CurrentPage - 1) * MaxRow)
                        .Take(MaxRow);

        
            return query.ToList();
        }

      public  List<ProductDTO> GetProductByCategoryID(out int total, int ? CategoryID){        

            var query = from product in _unitOfWork.GetRepository<Product>().GetQueryable()
                        join category in _unitOfWork.GetRepository<Category>().GetQueryable()
                        on new { cid = product.CategoryID } equals new { cid = category.ID }
                        where (product.CategoryID == CategoryID)
                        select new ProductDTO
                        {
                            ID = product.ID,
                            Name = product.Name,
                            CategoryID = product.CategoryID,
                            CategoryName = category.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Discount = (decimal)product.Discount,
                            Quantity = product.Quantity
                        };

            total = query.Count();
            return query.ToList();
        }

        public ProductDTO GetProductByID(int ProductID)
        {
            var query = from product in _unitOfWork.GetRepository<Product>().GetQueryable()
                        join category in _unitOfWork.GetRepository<Category>().GetQueryable()
                        on new { cid = product.CategoryID } equals new { cid = category.ID }
                        where  product.ID  == ProductID

                        select new ProductDTO
                        {
                            ID = product.ID,
                            Name = product.Name,
                            CategoryID = product.CategoryID,
                            CategoryName = category.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Discount = (decimal)product.Discount,
                            Quantity = product.Quantity
                        };


            return query.FirstOrDefault();
        }
    }
}
