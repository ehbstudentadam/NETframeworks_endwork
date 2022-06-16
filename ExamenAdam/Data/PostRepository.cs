using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;
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

        public IEnumerable<Post>? GetAllPoststFromUser(User user) => _context
            .Set<Post>()
            .Where(entity => entity.User == user)
            .ToList();

        public IEnumerable<Post>? GetAllPoststFromUserFromUntill(User user, int startIndex, int stopIndex) => _context
            .Set<Post>()
            .Where(entity => entity.User == user)
            .Skip(startIndex)
            .Take(stopIndex)
            .ToList();

        public void DeletePost(Post post)
        {
            _context.Set<Post>().Remove(post);
            _context.SaveChanges();
        }

    }
}
