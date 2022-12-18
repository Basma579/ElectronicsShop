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
    public class CategoryService : ICategoryService
    {

        private IUnitOfWork _unitOfWork;

      public  CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public DbOperationStatusEnum CreateCategory(CategoryDTO categoryDTO)
        {
            Category category = BusinessAutoMapper.Mapper.Map<CategoryDTO, Category>(categoryDTO);

            _unitOfWork.GetRepository<Category>().Insert(category);
            _unitOfWork.Commit();
            return DbOperationStatusEnum.Success;
        }

        public List<CategoryDTO> GetAllCategory(out int total)
        {
            var query = from category in _unitOfWork.GetRepository<Category>().GetQueryable()

                        select new CategoryDTO
                        {
                            Name = category.Name,
                            ID = category.ID
                        };
        
            total = query.Count();
            return query.ToList();
          }
    }
}
