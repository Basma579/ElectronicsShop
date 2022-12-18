using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ElectronicsShop.Repository.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> GetQueryable();

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        T GetById(object id);
        void Insert(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void Update(T entityToUpdate);
        void InsertBatch(IEnumerable<T> entities);
        void DeleteBatch(IEnumerable<T> entities);
        void UpdateBatch(IEnumerable<T> entities);

        IEnumerable<T> GetPaged(IEnumerable<Expression<Func<T, bool>>> filters,
                                IEnumerable<Expression<Func<T, object>>> orderCriterias,
                                IEnumerable<Expression<Func<T, object>>> descOrderCriterias, int pageNumber,
                                int pageSize, out int resultCount);

        void RefreshContext(T entity);
    }
}
