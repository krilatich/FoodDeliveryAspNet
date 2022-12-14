using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace food_delivery.Data.Models
{


    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public Guid Id { get; set; }
        public string fullName { get; set; }
         public string? birthDate { get; set; }
        public Gender gender { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }

        public ICollection<Order> orders { get; set; }

    }
}
