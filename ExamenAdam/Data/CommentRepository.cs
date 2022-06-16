using ExamenAdam.Entities;

namespace ExamenAdam.Data
{
    public class CommentRepository : BaseRepository<Comment>
    {
        ExamenAdamContext _context;
        public CommentRepository(ExamenAdamContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteComment(Comment comment)
        {
            _context.Set<Comment>().Remove(comment);
            _context.SaveChanges();
        }


    }
}
