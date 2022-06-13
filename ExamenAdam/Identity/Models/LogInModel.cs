using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Identity.Models
{
    public class LogInModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public string? RedirectUrl { get; set; } = "/";
    }
}
