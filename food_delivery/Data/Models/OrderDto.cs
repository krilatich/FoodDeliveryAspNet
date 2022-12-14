using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class OrderDto
    {
        public Guid id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public string deliveryTime { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public string orderTime { get; set; }
        [Required]
        public OrderStatus status { get; set; }
        [Required]
        public double price { get; set; }
        public ICollection<DishBasketDto> dishes { get; set; }
        [Required]
        [MinLength(1)]
        public string address { get; set; }

    }
}
