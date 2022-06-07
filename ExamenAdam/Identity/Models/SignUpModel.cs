using ExamenAdam.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Identity.Models
{
    public class SignUpModel
    {
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Sex { get; set; } = null!;
        [Required]
        public Address Address { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Approved { get; set; } = false;

        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;




    }
}
