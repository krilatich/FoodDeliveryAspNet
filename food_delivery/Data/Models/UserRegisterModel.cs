using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace food_delivery.Data.Models
{
    public class UserRegisterModel
    {
        [Required]
        [MinLength(1)]
        

        public string fullName { get; set; }
        [Required]
        [MinLength(6)]
        public string password { get; set; }

        [DataType(DataType.DateTime)]
        public string? birthDate { get; set; }
        [Required]
        public Gender gender { get; set; }
        public string? address { get; set; }

        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string email { get; set; }

        [Phone]
        public string? phoneNumber { get; set; }
    }
}
