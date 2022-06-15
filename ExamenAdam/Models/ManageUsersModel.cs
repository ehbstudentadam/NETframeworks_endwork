using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Models
{
    public class ManageUsersModel
    {
        public Dictionary<User, Role>? UnapprovedUsersAndRoles { get; set; }     
        public List<Role>? AllRoles { get; set; }
        public Dictionary<User, Role>? UserResult { get; set; }
        public int? SearchFrom { get; set; } = 0;
        public int? SearchTo { get; set; } = 10;
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
