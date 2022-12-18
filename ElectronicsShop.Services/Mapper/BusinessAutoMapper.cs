using AutoMapper;
using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Core.DTOs;


namespace ElectronicsShop.Services.Common.Mapper
{
    public class BusinessAutoMapper
    {
        public static void Configure()
        {
          

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Category, CategoryDTO>();
                cfg.CreateMap<CategoryDTO, Category>();
                cfg.CreateMap<ProductDTO, Product>();
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Order, OrderDTO>(); 
                cfg.CreateMap<OrderDTO, Order>();
                cfg.CreateMap<OrderDetails, OrderDetailsDTO>();
                cfg.CreateMap<OrderDetailsDTO, OrderDetails>();


            });

            Mapper = mapperConfig.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }
    }
}
