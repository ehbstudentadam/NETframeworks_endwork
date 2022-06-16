using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Models
{
    public class BlogIndexModel
    {
        public Dictionary<Post, string>? PostWithDescription { get; set; }
    }
}
