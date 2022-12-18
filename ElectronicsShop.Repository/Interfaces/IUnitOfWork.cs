using ElectronicsShop.Repository.Interfaces;

namespace ElectronicsShop.Repository.Interfaces
{
    public partial interface IUnitOfWork
    {
        #region Methods

        IRepositoryBase<T> GetRepository<T>() where T : class;

        void Dispose();

        void Commit();

        void ExecuteSqlCommand(string sql);

        void OpenTransaction();

        void CommitTransaction();

        void RollBackTransaction();

        #endregion Methods
    }
}