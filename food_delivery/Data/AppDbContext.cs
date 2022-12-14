using Microsoft.EntityFrameworkCore;

using food_delivery.Data.Models;

namespace food_delivery.Data
{
    public class AppDbContext:DbContext
    
    {

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<DishInOrder> DishInOrders { get; set; }
        public DbSet<DishBasket> DishBaskets { get; set; }
        public DbSet<LoginCredential> Credentials { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            Database.EnsureCreated();
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().Property(x => x.id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<LoginCredential>().HasData(
                 new LoginCredential { email = "user@example.com", password = "string" }

          );
            modelBuilder.Entity<Dish>().HasData(
          new Dish {Id = Guid.NewGuid(), 
              name = "firstWok",
              description = "firstWok description" ,
              dishCategory = DishCategory.Wok, 
              price = 500,
              vegetarian = false  },
           new Dish
           {
               Id = Guid.NewGuid(),
               name = "secondWok",
               description = "secondWok description",
               dishCategory = DishCategory.Wok,
               price = 750,
               vegetarian = true
           },
            new Dish
            {
                Id = Guid.NewGuid(),
                name = "thirdWok",
                description = "thirdWok description",
                dishCategory = DishCategory.Wok,
                price = 600,
                vegetarian = false
            },
             new Dish
             {
                 Id = Guid.NewGuid(),
                 name = "fourthWok",
                 description = "fourthWok description",
                 dishCategory = DishCategory.Wok,
                 price = 300,
                 vegetarian = false
             },
             new Dish
             {
                Id = Guid.NewGuid(),
                 name = "5Wok",
                 description = "5Wok description",
                 dishCategory = DishCategory.Wok,
                 price = 1100,
                 vegetarian = false
             },
             new Dish
             {
                 Id = Guid.NewGuid(),
                 name = "6Wok",
                 description = "6Wok description",
                 dishCategory = DishCategory.Wok,
                 price = 650,
                 vegetarian = true
             },
             new Dish
             {
                 Id = Guid.NewGuid(),
                 name = "7Wok",
                 description = "7Wok description",
                 dishCategory = DishCategory.Wok,
                 price = 830,
                 vegetarian = true
             }




          );

            modelBuilder.Entity<DishBasket>().HasKey(x=> new {x.DishId,x.UserId});

            modelBuilder.Entity<DishInOrder>().HasKey(x => new { x.OrderID, x.DishID});

            modelBuilder.Entity<Rating>().HasKey(x => new { x.UserID, x.DishID });



        } 




    }
}
