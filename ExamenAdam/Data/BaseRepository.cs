using ExamenAdam.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenAdam.Data
{
    public class BaseRepository<TEntity> where TEntity : Entity
    {
        public ExamenAdamContext _context { get; }

        public BaseRepository(ExamenAdamContext context)
        {
            _context = context;
        }

        public virtual TEntity? FindById(long id) => _context
            .Set<TEntity>()
            .Where(entity => entity.Id == id)
            .FirstOrDefault();

        public virtual IEnumerable<TEntity>? GetLastXAmount(int quantity) => _context
            .Set<TEntity>()
            .OrderByDescending(entity => entity.Id)
            .Take(quantity)
            .ToList();

        public void AddEntity(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

    }
}
