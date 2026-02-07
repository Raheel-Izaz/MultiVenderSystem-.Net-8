using System.Linq.Expressions;

namespace MultiVender.Application.Interfaces
{
    public interface IRepository<T> where T :  class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(int id);
        Task AddAsync(T entity);
        void Remove(T entity);
    }
}
