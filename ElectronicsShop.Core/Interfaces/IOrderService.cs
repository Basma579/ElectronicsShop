using ElectronicsShop.Core.DTOs;
using ElectronicsShop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsShop.Core.Interfaces
{
   public interface IOrderService
    {
       List<DisplayedOrderDTO> GetUserOrders(out int total , string UserID);
        DbOperationStatusEnum CreateOrder(OrderDTO orderDTO );

    }
}
