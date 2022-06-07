using ExamenAdam.Entities;

namespace ExamenAdam.Data
{
    public class PostRepository : BaseRepository<Post>
    {
        ExamenAdamContext _context;

        public PostRepository(ExamenAdamContext context) : base(context)
        {
            this._context = context;
        }
    }
}
