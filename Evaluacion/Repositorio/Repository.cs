using BiblotecaClase.Datos;
using Evaluacion.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Evaluacion.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BackendContext _db;
        internal DbSet<T> dbSet;

        public Repository(BackendContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task Create(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await Save();
        }
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> Gett(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);

            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GettAll(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);

            }
            return await query.ToListAsync();

        }

        public async Task Remove(T entidad)
        {
            dbSet.Remove(entidad);
            await Save();
        }
    }
}
