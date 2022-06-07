using ExamenAdam.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExamenAdam.Identity.Entities
{
    public class User : IdentityUser<long>
    {
        public DateTime Birthday { get; set; }
        public string Sex { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool Approved { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        
        public List<Post> Posts { get; set; } = new List<Post>();
        
    }
}
