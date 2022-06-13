using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Models
{
    public class BlogPostModel
    {
/*      public long PostId { get; set; }
        public string PostTitle { get; set; } = null!;
        public string PostBody { get; set; } = null!;
        public List<Comment> Comments { get; set; } = new List<Comment>();*/
        public string NewComment { get; set; } = null!;
        public Post Post { get; set; } = null!;
        

    }
}
