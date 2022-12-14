
using food_delivery.Data;
using food_delivery.Data.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Security.Principal;

namespace food_delivery.Services
{
    public interface IFoodDeliveryService
    {
        Task<IActionResult> RegUser(UserRegisterModel model);

        Task EditUser(string email, UserEditModel model);
        Task<UserDto> GetProfile(string email);

       Task<Dish?> GetDish(Guid id);

        Task<bool> Check(Guid dishId, string email);

        Task CreateOrder(CreateOrderDto model, string email);

        Task<int> ConfirmOrder(Guid id, string email);

        Task<int> setRate(Guid dishId, string email, double ratingScore);

        Task<IActionResult> GetDishes
            (
            DishCategory[] categories,
            DishSorting sorting,
            int page,
            int pageSize,
            bool vegetarian
            );


        Task<IEnumerable<DishBasketDto>> GetBasket(string email);
        Task<ClaimsIdentity> GetIdentity(string email, string password);

        Task addDish(Guid dishId, string email);
        Task deleteDish(Guid dishId, bool increase, string email);

        Task <OrderDto> OrderDetails(Guid id,string email);

        Task<IEnumerable<OrderInfo>> GetOrders(string email);

    }


    public class FoodDeliveryService : IFoodDeliveryService
    {
        private readonly AppDbContext _context;

        public FoodDeliveryService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> RegUser(UserRegisterModel model)
        {
            await _context.Users.AddAsync(new User
            {

                fullName = model.fullName,
                address = model.address,
                birthDate = model.birthDate,
                email = model.email,
                phoneNumber = model.phoneNumber,
                gender = model.gender,
            });

            await _context.Credentials.AddAsync(new LoginCredential
            {
                email = model.email,
                password = model.password,
            });
            await _context.SaveChangesAsync();

            var identity = await GetIdentity(model.email, model.password);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfigurations.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
            };

            return new JsonResult(response);


        }

        public async Task<IEnumerable<DishBasketDto>> GetBasket(string email)
        {
            var user = await GetProfile(email);



            var dishBaskets = _context.DishBaskets.Where(x => x.UserId == user.Id).Join(_context.Dishes,
        u => u.DishId,
        d => d.Id,
        (u, d) => new DishBasketDto
        {

            Id = d.Id,
            name = d.name,
            amount = u.amount,
            price = d.price,
            totalPrice = d.price * u.amount,
            image = d.image,

        }).ToList();


            return dishBaskets;

        }

        public async Task addDish(Guid dishId, string email)
        {

            var userId = (await GetProfile(email)).Id;

            var dish = await _context.Dishes.FindAsync(dishId);

            var dishBasket = await _context.DishBaskets.FirstOrDefaultAsync(x => x.UserId == userId && x.DishId == dishId);

            if (dishBasket != null)
            {

                dishBasket.amount++;


            }

            else
            {

                await _context.DishBaskets.AddAsync(new DishBasket
                {
                    DishId = dishId,
                    UserId = userId,
                    amount = 1

                });

            }


            await _context.SaveChangesAsync();


        }


        public async Task deleteDish(Guid dishId, bool increase, string email)
        {
            var userId = (await GetProfile(email)).Id;

            var dishBasket = await _context.DishBaskets.SingleOrDefaultAsync
                (x => x.DishId == dishId && x.UserId == userId);

            if (!increase)
            {
                _context.DishBaskets.Remove(dishBasket);

            }
            else
            {
                dishBasket.amount--;
                if (dishBasket.amount == 0) _context.DishBaskets.Remove(dishBasket);
            }


            await _context.SaveChangesAsync();

        }

        public async Task<UserDto> GetProfile(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == email);
            return new UserDto
            {
                email = user.email,
                phoneNumber = user.phoneNumber,
                address = user.address,
                birthDate = user.birthDate,
                fullName = user.fullName,
                gender = user.gender,
                Id = user.Id,
            };

        }

        public async Task EditUser(string email, UserEditModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.email == email);

            user.address = model.address;
            user.phoneNumber = model.phoneNumber;
            user.fullName = model.fullName;
            user.birthDate = model.birthDate;
            user.gender = model.gender;

            await _context.SaveChangesAsync();

        }



        public async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            //  var user =  await _context.Credentials.FirstOrDefault(x => x.email == email && x.password == password);

            var user = await _context.Credentials.FindAsync(email);

            if (user == null)
            {
                return null;
            }
            if (user.password != password) return null;

            // Claims описывают набор базовых данных для авторизованного пользователя
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.email),
                    //new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };

            //Claims identity и будет являться полезной нагрузкой в JWT токене, которая будет проверяться стандартным атрибутом Authorize
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public async Task<Dish> GetDish(Guid id)
        {
            return await _context.Dishes.FindAsync(id);


        }

        public async Task<IActionResult> GetDishes(
            DishCategory[] categories,
            DishSorting sorting,
            int page,
            int pageSize,
            bool vegetarian
        )
        {


            IEnumerable<Dish> dishesPerPages =
                    _context.Dishes.Where(x => categories.Contains(x.dishCategory) &&
                    (x.vegetarian == true || vegetarian == x.vegetarian));

            switch (sorting)
            {
                case DishSorting.NameAsc:
                    dishesPerPages = dishesPerPages.OrderBy(x => x.name)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case DishSorting.NameDesc:
                    dishesPerPages = dishesPerPages.OrderByDescending(x => x.name)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case DishSorting.PriceDesc:
                    dishesPerPages = dishesPerPages.OrderByDescending(x => x.price)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case DishSorting.RatingDesc:
                    dishesPerPages = dishesPerPages.OrderByDescending(x => x.rating)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case DishSorting.PriceAsc:
                    dishesPerPages = dishesPerPages.OrderBy(x => x.price)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case DishSorting.RatingAsc:
                    dishesPerPages = dishesPerPages.OrderBy(x => x.rating)
                        .Skip((page - 1) * pageSize).Take(pageSize);
                    break;

            }






            PageInfo pageInfo = new PageInfo
            {
                current = page,
                size = pageSize,
                count = (int)Math.Ceiling((decimal)dishesPerPages.Count() / pageSize)
            };



            var response = new
            {
                dishes = dishesPerPages,
                pagination = pageInfo
            };

            return new JsonResult(response);

        }
        public double CalculatePrice(List<DishBasketDto> dishBaskets)
        {
            double price = 0;
            dishBaskets.ForEach(d => { price += d.totalPrice; });
            return price;
        }
        public async Task CreateOrder(CreateOrderDto model, string email)
        {


            List<DishBasketDto> dishes = (await GetBasket(email)).ToList();

            var order = new Order
            {
                deliveryTime = model.deliveryTime,
                status = OrderStatus.InProcess,
                address = model.address,
                id = Guid.NewGuid(),
                price = CalculatePrice(dishes),
                UserId = (await GetProfile(email)).Id,
                orderTime = DateTime.Now.ToString(),


            };

            await _context.Orders.AddAsync(order);


            for (int i = 0; i < dishes.Count; i++)
            {
                await _context.DishInOrders.AddAsync(new DishInOrder
                {
                    DishID = dishes[i].Id,
                    OrderID = order.id,
                    amount = dishes[i].amount
                });
                var dishBasket = await _context.DishBaskets.FirstOrDefaultAsync
                    (x => x.DishId == dishes[i].Id && x.UserId == order.UserId);


                _context.DishBaskets.Remove(dishBasket);
            }

            await _context.SaveChangesAsync();

        }

        public async Task<ICollection<DishBasketDto>> getDishesFromOrder(Guid id)
        {
            var dishes = _context.DishInOrders.Where(x => x.OrderID == id).ToList();

            List<DishBasketDto> dishBasketDtos = new List<DishBasketDto>();

            for (int i = 0; i < dishes.Count; i++)
            {
                var dish = await _context.Dishes.FindAsync(dishes[i].DishID);
                dishBasketDtos.Add(new DishBasketDto
                {
                    Id = dish.Id,
                    name = dish.name,
                    amount = dishes[i].amount,
                    price = dish.price,
                    totalPrice = dish.price * dishes[i].amount,
                    image = dish.image,

                });

            }




            return dishBasketDtos;

        }
        public async Task<OrderDto> OrderDetails(Guid id, string email)
        {
            var order = await _context.Orders.FindAsync(id);
            var userId = (await GetProfile(email)).Id;
            if (order.UserId != userId) return new OrderDto();

            return new OrderDto
            {
                status = order.status,
                address = order.address,
                deliveryTime = order.deliveryTime,
                orderTime = order.orderTime,
                id = order.id,
                price = order.price,
                dishes = await getDishesFromOrder(order.id),


            };

        }

        public async Task<IEnumerable<OrderInfo>> GetOrders(string email)
        {
            var userId = (await GetProfile(email)).Id;

            List<OrderInfo> orderInfos = new List<OrderInfo>();

            _context.Orders.Where(x => x.UserId == userId).ToList().ForEach(x => orderInfos.Add(
                new OrderInfo
                {
                    orderTime = x.orderTime,
                    deliveryTime = x.deliveryTime,
                    status = x.status,
                    Id = x.id,
                    price = x.price,

                })

            );




            return orderInfos;
        }

        public async Task<int> ConfirmOrder(Guid id, string email)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order.UserId != (await GetProfile(email)).Id) return StatusCodes.Status403Forbidden;

            order.status = OrderStatus.Delivered;


            await _context.SaveChangesAsync();

            return StatusCodes.Status200OK;
        }

        public async Task<bool> Check(Guid dishId, string email)
        {
            var userId = (await GetProfile(email)).Id;

            var orderIds = new List<Guid> (); 
               
            _context.Orders.Where(x => x.UserId == userId).ToList()
                .ForEach(x=> orderIds.Add(x.id));

            
           
                var dishInOrder = _context.DishInOrders
                    .Where(x => orderIds.Contains(x.OrderID)  && x.DishID == dishId);

                
            if (dishInOrder.Any())  return true; 

            


            return false;

        }

        public async Task<int> setRate(Guid dishId, string email, double ratingScore)
        {
            var userId = (await GetProfile(email)).Id;

            var dish = await _context.Dishes.FindAsync(dishId);

            if (dish == null) return StatusCodes.Status404NotFound;


            var rating = _context.Rating.Where(x=> x.UserID == userId && x.DishID == dishId);

            if (rating.Any())
            {

                rating.First().RatingScore = ratingScore;

            }

            else
            {
                await _context.Rating.AddAsync(new Rating
                {
                    DishID = dish.Id,
                    UserID = userId,
                    RatingScore = ratingScore
                });

            }

                await _context.SaveChangesAsync();

           

            double sum = 0; int total = 0;
            await _context.Rating.Where(x=>x.DishID == dishId).ForEachAsync(x =>
            {
                sum += x.RatingScore;
                total++;

            });

            dish.rating = (double)(sum/total);

            await _context.SaveChangesAsync();



            return StatusCodes.Status200OK;

        }
    }

    
}
