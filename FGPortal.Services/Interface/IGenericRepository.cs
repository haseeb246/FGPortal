using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FGPortal.Services
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
    }
}
