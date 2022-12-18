using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ElectronicsShop.Core.DTOs;
using ElectronicsShop.ViewModels;

namespace ElectronicShope.Mapper
{
    public static class WebAutoMapper
    {
        public static void Configure()
        {

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => {

                cfg.CreateMap<ProductViewModel, ProductDTO>();
                cfg.CreateMap<ProductDTO, ProductViewModel>();
                cfg.CreateMap<DisplayedOrderDTO, OrderViewModel>(); 
                cfg.CreateMap<OrderViewModel, DisplayedOrderDTO>();
                cfg.CreateMap<CategoryDTO, CategoryViewModel>();
                cfg.CreateMap<CategoryViewModel, CategoryDTO>();


            });

            Mapper = mapperConfig.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }
    }
}
