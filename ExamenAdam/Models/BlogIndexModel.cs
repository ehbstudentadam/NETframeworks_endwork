using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Models
{
    public class BlogIndexModel
    {
        public Dictionary<Post, string> PostWithDescription { get; set; } = null!;

/*        public long PostId { get; set; }
        public string PostTitle { get; set; } = null!;
        public string FirstFewBodySentences { get; set; } = null!;
        public User WrittenByUser { get; set; } = null!;*/

    }
}
