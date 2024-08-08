
using FPV.API.Core.Repositories.Generic;
using FPV.API.Core.Repositories.Generic.Interfaces;
using FPV.API.Core.Context;
using FPV.API.Core.Repositories.GenericWorker.Interfaces;

namespace FPV.API.Core.Repositories.GenericWorker
{
    public class GenericWorker : IGenericWorker
    {
        protected readonly AmarisDbContext _dbContext;
        private IDictionary<Type, dynamic> _repositories;
        public GenericWorker(AmarisDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, dynamic>();
        }
        public IGenericRepository<T> Repository<T>() where T : class
        {
            var entityType = typeof(T);
            if (_repositories.ContainsKey(entityType))
            {
                return _repositories[entityType];
            }
            var repositoryType = typeof(GenericRepository<>);
            var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
            _repositories.Add(entityType, repository);
            return (IGenericRepository<T>)repository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task RollBackChangesAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

    }
}
