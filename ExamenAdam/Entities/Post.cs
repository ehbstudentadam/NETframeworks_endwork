using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenAdam.Entities
{
    public class Post : Entity
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;


        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;


        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
