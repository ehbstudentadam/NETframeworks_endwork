using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Identity.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Password { get; set; } = null!;
        public string? RedirectUrl { get; set; } = "/";
    }
}
