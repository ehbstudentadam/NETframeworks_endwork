using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Models
{
    public class EditUserAndRoleModel
    {
        [Required]
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        [Required]
        public bool Approved { get; set; }
        [Required]
        public Role RoleEdit { get; set; } = null!;
        public List<Role>? AllRoles { get; set; }
    }
}
