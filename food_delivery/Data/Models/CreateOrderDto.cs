using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class CreateOrderDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public string deliveryTime { get; set; }
        [Required]
        [MinLength(1)]
        public string address { get; set; }
    }
}
