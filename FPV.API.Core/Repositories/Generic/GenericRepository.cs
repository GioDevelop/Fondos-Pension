using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FPV.API.Core.Repositories.Generic.Interfaces;
using FPV.API.Core.Context;   
using FPV.API.Core.Repositories.Generic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Properties

        public readonly AmarisDbContext _context;
        #endregion

        #region Constructor
        public GenericRepository(AmarisDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetByFilterByIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = this._context.Set<T>().Where(match).AsNoTracking();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }
            return queryable;
        }

        public IQueryable<T> GetByFilterOrderByIncluding(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public async Task<List<T>> GetListByFilterOrderByIncludingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter).AsNoTracking();

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include<T, object>(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<T?> GetFirstByFilterOrderByIncludingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter).AsNoTracking();

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include<T, object>(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            int rows = await _context.SaveChangesAsync();
            return rows > 0 ? entity : null;
        }

        public async Task<bool> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            int rows = await _context.SaveChangesAsync();
            return rows > 0 ? true : false;
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            int rows = await _context.SaveChangesAsync();
            return rows > 0 ? true : false;
        }

        public async Task<bool> DeleteById(object id)
        {
            T entity = await GetById(id);
            _context.Set<T>().Remove(entity);
            int rows = await _context.SaveChangesAsync();
            return rows > 0 ? true : false;
        }

        /// <summary>
        /// Borra uno o varios registros segun el filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByFilter(Expression<Func<T, bool>> filter)
        {
            var entities = _context.Set<T>().Where(filter);
            _context.Set<T>().RemoveRange(entities);
            int rows = await _context.SaveChangesAsync();
            return rows > 0 ? true : false;
        }
        public async Task Add2(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        //public async Task<T> AddAsync(T entity)
        //{
        //    await _context.Set<T>().AddAsync(entity);
        //    return entity;
        //}
        public void Update2(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete2(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        #endregion



        #endregion

    }
}
