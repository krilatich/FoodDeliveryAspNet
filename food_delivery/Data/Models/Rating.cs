namespace food_delivery.Data.Models
{
    public class Rating
    {
       public Guid DishID { get; set; }
       
        public Guid UserID { get; set; }

        public double RatingScore { get; set; }

    }
}
