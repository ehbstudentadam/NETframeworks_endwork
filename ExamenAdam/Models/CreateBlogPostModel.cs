using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Models
{
    public class CreateBlogPostModel
    {
        [Required, MinLength(10)]
        public string Title { get; set; } = null!;
        [Required, MinLength(500)]
        public string Body { get; set; } = null!;

    }
}
