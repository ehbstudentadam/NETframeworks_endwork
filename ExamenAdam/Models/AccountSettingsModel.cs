using ExamenAdam.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Models
{
    public class AccountSettingsModel
    {
        public List<Post>? Posts { get; set; }
        [Required]
        public long AccountId { get; set; }
        public int? SearchFrom { get; set; } = 0;
        public int? SearchTo { get; set; } = 10;
    }
}
