using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Repository.Implementation
{
    public class EFRepository<T> : IRepositoryBase<T> where T : class
    {
        private E_Shopping_DbContext _e_Shopping_DbContext;
        private DbSet<T> _dbSet;


        public EFRepository(E_Shopping_DbContext dataContext)
        {
            _e_Shopping_DbContext = dataContext;
            _dbSet = dataContext.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet;
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_e_Shopping_DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _e_Shopping_DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void InsertBatch(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void DeleteBatch(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void UpdateBatch(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual IEnumerable<T> GetWithRawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual object ExecuteStoredProcedure(string query, params object[] parameters)
        {
            var result = _e_Shopping_DbContext.Database.ExecuteSqlRaw(query, parameters);

            return result;
        }

        public virtual IEnumerable<T> GetPaged(IEnumerable<Expression<Func<T, bool>>> filters,
                                               IEnumerable<Expression<Func<T, object>>> orderCriterias,
                                               IEnumerable<Expression<Func<T, object>>> descOrderCriterias,
                                               int pageNumber, int pageSize, out int resultCount)
        {
            IQueryable<T> query = _dbSet;
            if (filters != null)
                foreach (var filter in filters)
                {
                    if (filter == null) continue;

                    query = query.Where(filter);
                }
            bool pagingEnabled = pageSize > 0;

            resultCount = query.Count();

            if (orderCriterias != null)
                foreach (var orderCriteria in orderCriterias)
                {
                    if (orderCriteria == null) continue;
                    query = query.OrderBy(orderCriteria);
                }

            if (descOrderCriterias != null)
                foreach (var descOrderCriteria in descOrderCriterias)
                {
                    if (descOrderCriteria == null) continue;
                    query = query.OrderByDescending(descOrderCriteria);

                }

            if (pagingEnabled)
                query = query.Skip(pageSize * (pageNumber)).Take(pageSize);

            return query.ToList();
        }


        public void RefreshContext(T entity)
        {
            if (entity != null)
                foreach (var entityObj in _e_Shopping_DbContext.ChangeTracker.Entries().Where(entityObj => entity.Equals(entityObj.Entity)))
                {
                    entityObj.State = EntityState.Detached;
                }
        }

    }
}
