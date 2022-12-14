using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class LoginCredential
    {
        [Required]
        [EmailAddress]
        [MinLength(1)]
        [Key]
        public string email { get; set; }
        [Required]
        [MinLength(1)]
        public string password { get; set; }
    }
}
