using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ExamenAdam.Entities
{
    public class Address : Entity
    {
        [Required]
        public string StreetName { get; set; } = null!;
        [Required]
        public string StreetNumber { get; set; } = null!;
        [ValidateNever]
        public string PostalBus { get; set; } = "/";
        [Required]
        public string PostalCode { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
    }
}
