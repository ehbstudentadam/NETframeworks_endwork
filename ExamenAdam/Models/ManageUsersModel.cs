using ExamenAdam.Identity.Entities;

namespace ExamenAdam.Models
{
    public class ManageUsersModel
    {
        public Dictionary<User, Role> UnapprovedUsersAndRoles { get; set; } = null!;
        public List<Role> AllRoles { get; set; } = null!;







    }
}
