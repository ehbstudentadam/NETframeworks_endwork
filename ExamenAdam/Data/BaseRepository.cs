using ExamenAdam.Entities;

namespace ExamenAdam.Data
{
    public class BaseRepository<TEntity> where TEntity : Entity
    {
        public ExamenAdamContext _context { get; }

        public BaseRepository(ExamenAdamContext context)
        {
            _context = context;
        }

    }
}
