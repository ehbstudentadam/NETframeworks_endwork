using ExamenAdam.Entities;
using ExamenAdam.Identity.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Models
{
    public class BlogPostModel
    {
        [Required, MinLength(15)]
        public string NewComment { get; set; } = null!;
        public Post? Post { get; set; }        

    }
}
