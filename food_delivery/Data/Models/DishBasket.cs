namespace food_delivery.Data.Models
{
    public class DishBasket
    {
   
        public Guid UserId { get; set; }
        public User User { get; set; }  

        public Guid DishId { get; set; }
        public Dish Dish { get; set; }



        public int amount{ get; set; }



    }
}
