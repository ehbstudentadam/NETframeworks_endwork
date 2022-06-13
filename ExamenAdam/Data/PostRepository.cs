using ExamenAdam.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenAdam.Data
{
    public class PostRepository : BaseRepository<Post>
    {
        ExamenAdamContext _context;

        public PostRepository(ExamenAdamContext context) : base(context)
        {
            this._context = context;
        }

        public override Post? FindById(long id) => _context
            .Set<Post>()
            .Include(x => x.Comments)
            .Include(y => y.User)
            .Where(entity => entity.Id == id)
            .FirstOrDefault();

        public override IEnumerable<Post>? GetLastXAmount(int quantity) => _context
            .Set<Post>()
            .Include(x => x.Comments)
            .Include(y => y.User)
            .OrderByDescending(entity => entity.Id)
            .Take(quantity)
            .ToList();

    }
}
