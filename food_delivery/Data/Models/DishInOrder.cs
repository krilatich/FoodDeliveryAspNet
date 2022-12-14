namespace food_delivery.Data.Models
{
    public class DishInOrder
    {

        public Guid OrderID { get; set; }

        public Guid DishID { get; set; }

        public int amount { get; set; }


    }
}
