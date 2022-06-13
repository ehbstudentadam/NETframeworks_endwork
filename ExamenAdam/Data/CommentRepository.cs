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

/*        public void AddNewComment(Post post, string comment)
        {
            _context.Comments.Add(new Comment { Commentary = comment, Post = post });
            _context.SaveChanges();
        }*/


    }
}
