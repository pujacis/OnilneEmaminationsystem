using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnilneExa.DataAccessLyer.Repository
{
    public interface IGenericRepository<T>:IDisposable
    {
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
               string includeProperties = "");
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Add(T entity);
        Task<T> AddAsync(T entity); 
        void DeletebById(object id);
        void Delete(T entityToDelete);
        void Update(T entityToUpodate);
        Task<T> UpdateAsync(T entityToUpodate);
        Task<T> DeleteAsync(T entityToDelete);
            
}
