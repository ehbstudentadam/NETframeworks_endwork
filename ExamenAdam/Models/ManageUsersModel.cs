using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Models
{
    public class ManageUsersModel
    {
        public List<User> SearchResultUsers { get; set; } = null!;
        public List<User> PendingUsers { get; set; } = null!;




    }
}
