using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class OrderInfo
    {

        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string deliveryTime { get; set;}

        [Required]
        [DataType(DataType.DateTime)]
        public string orderTime { get; set;}
        [Required]
        public OrderStatus status { get; set;}
        [Required]
        public double price { get; set;}
    }
}
