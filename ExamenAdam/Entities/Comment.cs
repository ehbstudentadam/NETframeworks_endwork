using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamenAdam.Entities
{
    public class Comment : Entity
    {
        public string Commentary { get; set; } = null!;


        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;



        public long PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; } = null!;
    }
}
