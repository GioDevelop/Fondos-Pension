using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FPV.API.Core.Repositories.Generic.Interfaces
{
    public interface IGenericRepository<T> where T : class  /*BaseEntity*/
    {
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(object id);
        IQueryable<T> GetByFilterByIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetByFilterOrderByIncluding(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetListByFilterOrderByIncludingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstByFilterOrderByIncludingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<T?> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> DeleteById(object id);
        Task<bool> DeleteByFilter(Expression<Func<T, bool>> filter);

        Task Add2(T entity);
        void Update2(T entity);
        void Delete2(T entity);


    }
}
