using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class UserEditModel
    {

        [Required]
        [MinLength(1)]


        public string fullName { get; set; }
        
        [DataType(DataType.DateTime)]
        public string? birthDate { get; set; }
        [Required]
        public Gender gender { get; set; }
        public string? address { get; set; }

        [Phone]
        public string? phoneNumber { get; set; }
    }
}
