using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace food_delivery.Data.Models
{
    public class Dish
    {


       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [MinLength(1)]
        [Required]
        public string name { get; set; }
        public string? description { get; set; }

        [Required]
        public double price { get; set; }

        public string? image { get; set; }



        public bool vegetarian { get; set; }

        public double? rating { get; set;}
        
        public DishCategory dishCategory { get; set; }

       







    }
}
