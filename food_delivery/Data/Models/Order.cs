using System.ComponentModel.DataAnnotations;

namespace food_delivery.Data.Models
{
    public class Order
    {
        public Guid id { get; set; }
      
        public string deliveryTime { get; set; }
     
        public string orderTime { get; set; }
 
        public OrderStatus status { get; set; }
   
        public double price { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
     
        public string address { get; set; }
    }
}
