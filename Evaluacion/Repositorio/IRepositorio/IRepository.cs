using System.Linq.Expressions;

namespace Evaluacion.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task<List<T>> GettAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Gett(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Remove(T entity);
        Task Save();
    }
}
