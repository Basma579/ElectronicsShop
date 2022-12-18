using System;
using System.Collections.Generic;
using System.Text;
using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Core;
using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using ElectronicsShop.Core.Interfaces;
using ElectronicsShop.Repository.Interfaces;
using System.Collections;
using System.Linq;
using ElectronicsShop.Services.Common.Mapper;

namespace ElectronicsShop.Services.AdminServices
{
    public class OrderService : IOrderService
    {

        private IUnitOfWork _unitOfWork;

      public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
     
        public DbOperationStatusEnum CreateOrder(OrderDTO orderDTO )
        {
            Order order = BusinessAutoMapper.Mapper.Map<OrderDTO, Order>(orderDTO);

            _unitOfWork.GetRepository<Order>().Insert(order);
            _unitOfWork.Commit();
            return DbOperationStatusEnum.Success;
        }

        public List<DisplayedOrderDTO> GetUserOrders(out int total, string UserID)
        {
            var query = from order in _unitOfWork.GetRepository<Order>().GetQueryable()
                        join details in _unitOfWork.GetRepository<OrderDetails>().GetQueryable()
                        on order.ID equals details.OrderID
                        join product in _unitOfWork.GetRepository<Product>().GetQueryable()
                                on details.ProductID equals product.ID
                        where order.UserId== UserID

                        select new DisplayedOrderDTO
                        {
                     ID = order.ID , 
                     Description =product.Description , 
                     Name=product.Name,
                     Price= details.Price,
                     Discount=details.DiscountValue ,
                     PriceAfterDiscount= details.PriceAfterDiscount,
                     Quantity= details.Quantity, 
                     Total= order.Total
                        };

            total = query.Count();
            return query.ToList();
            //
        }

            //public List<CategoryDTO> GetAllCategory(out int total)
            //{
            //    var query = from category in _unitOfWork.GetRepository<Category>().GetQueryable()

            //                select new CategoryDTO
            //                {
            //                    Name = category.Name,
            //                    ID = category.ID
            //                };

            //    total = query.Count();
            //    return query.ToList();
            //  }
        }
    }
