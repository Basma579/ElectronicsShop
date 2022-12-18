using ElectronicShope.DBModel.DBModel;
using ElectronicsShop.Repository.Implementation;
using ElectronicsShop.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace ElectronicsShop.Repository.Implementation
{
    public partial class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private Variables

        private E_Shopping_DbContext context = null;
        private bool disposed = false;

        #endregion Private Variables

        #region Constructors

        public UnitOfWork(E_Shopping_DbContext context)
        {
            this.context = context;
            _repositoriesDictionary = new Dictionary<string, object>();
        }

        //public UnitOfWork()
        //{
        //    // since we are using "per Request life time manager" we can create
        //    // new dbcontext instance with unit of work instance, so only one dbcontext will exist per request
        //    this.context = new DiabeticAssessmentDbContext();
        //}

        #endregion Constructors

        private Dictionary<string, object> _repositoriesDictionary;

        #region Public Methods

        public IRepositoryBase<T> GetRepository<T>() where T : class
        {
            if (_repositoriesDictionary.ContainsKey(typeof(T).FullName ?? string.Empty))
            {
                return _repositoriesDictionary.GetValueOrDefault(typeof(T).FullName) as IRepositoryBase<T>;
            }

            var repository = new EFRepository<T>(context);
            _repositoriesDictionary.Add(typeof(T).FullName, repository);
            return repository;
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ExecuteSqlCommand(string sql)
        {
            this.context.Database.ExecuteSqlCommand(sql);
        }

        #endregion Public Methods

        #region Proptected Virtual Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }

        #endregion Proptected Virtual Methods

        #region TransactionHandling

        private IDbContextTransaction _transaction;

        public virtual void OpenTransaction()
        {
            _transaction = context.Database.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public virtual void RollBackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        #endregion TransactionHandling

    }
}