using food_delivery.Data.Models;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Security.Principal;


namespace food_delivery.Controllers
{

    /*
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    [ApiController]
    
    public class FoodDeliveryController: ControllerBase
    {
        private IFoodDeliveryService _foodDeliveryService;

        public FoodDeliveryController(IFoodDeliveryService foodDeliveryService)
        {
            _foodDeliveryService = foodDeliveryService;
        }

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };



        [HttpGet]
        [Route("/api/basket")]
        public IEnumerable<WeatherForecast> Get() {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

    }
    */

    [Route("api/account/")]
    [ApiController]
    public class UsersController : ControllerBase
    {


        private IFoodDeliveryService _foodDeliveryService;

        public UsersController(IFoodDeliveryService foodDeliveryService)
        {
            _foodDeliveryService = foodDeliveryService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Token([FromBody] LoginCredential model)
        {

            if (!ModelState.IsValid) //Проверка полученной модели данных
            {
                return StatusCode(401, "Model is incorrect");
            }
            try
            {

                var identity = await _foodDeliveryService.GetIdentity(model.email, model.password);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

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
                    access_token = encodedJwt,
                    username = identity.Name
                };

                return new JsonResult(response);

            }

            catch (Exception ex)
            {
                return StatusCode(500, "InternalServerError");
            }
        }





        [HttpPost("register")]
        public async Task<IActionResult> Post(UserRegisterModel model)
        {
            if (!ModelState.IsValid) //Проверка полученной модели данных
            {
                return StatusCode(401, "Model is incorrect");
            }

            try
            {

                return Ok(await _foodDeliveryService.RegUser(model));
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }


        [HttpGet("profile"), Authorize]

        public async Task<ActionResult<User>> Get()
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(await _foodDeliveryService.GetProfile(email));
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }



        [HttpPost("logout"), Authorize]
        public IActionResult Logout()
        {
            return Ok();
        }


        [HttpPut("profile"), Authorize]
        public async Task<IActionResult> Put(UserEditModel userEdit)
        {

            if (!ModelState.IsValid) //Проверка полученной модели данных
            {
                return StatusCode(401, "Model is incorrect");
            }

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                await _foodDeliveryService.EditUser(email, userEdit);
                return Ok();
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }
    }


    [Route("api/basket/")]
    [ApiController]
    public class BasketController : ControllerBase
    {


        private IFoodDeliveryService _foodDeliveryService;
        // private readonly SignInManager<IdentityUser> _signInManager;

        public BasketController(IFoodDeliveryService foodDeliveryService)
        {
            _foodDeliveryService = foodDeliveryService;
        }



        [HttpGet, Authorize]

        public async Task<ActionResult<IEnumerable<DishBasketDto>>> Get()
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(await _foodDeliveryService.GetBasket(email));
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }


        [HttpPost("dish/{dishId}"), Authorize]

        public async Task<IActionResult> Post(Guid dishId)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                await _foodDeliveryService.addDish(dishId, email);
                return Ok();
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }


        [HttpDelete, Authorize]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> Delete(Guid dishId, bool increase = false)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                await _foodDeliveryService.deleteDish(dishId, increase, email);
                return Ok();
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }


    }


    [Route("api/dish/")]
    [ApiController]
    public class DishController : ControllerBase
    {


        private IFoodDeliveryService _foodDeliveryService;


        public DishController(IFoodDeliveryService foodDeliveryService)
        {
            _foodDeliveryService = foodDeliveryService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> Get(Guid id)
        {

            try
            {

                var dish = await _foodDeliveryService.GetDish(id);
                if (dish == null) return NotFound($"Dish with id =  {id} not found");
                return Ok(dish);
            }

            catch (Exception e)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }



        [Authorize]
        [HttpGet("{id}/rating/check")]
        public async Task<ActionResult<bool>> GetCheck([FromRoute] Guid id)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                return Ok(await _foodDeliveryService.Check(id, email));
                
            }

            catch (Exception e)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }


        [HttpGet]
        public async Task<IActionResult> Get
            (
            [FromQuery]
            DishCategory[] categories,
            DishSorting sorting,
            int page,
            bool vegetarian = false
        )
        {

            try
            {
                int pageSize = 5;
               
                return await _foodDeliveryService.
                    GetDishes(categories,sorting,page,pageSize,vegetarian);
            }

            catch (Exception e)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }



        [Authorize]
        [HttpPost("{id}/rating")]
        public async Task<IActionResult> Post(Guid id, [FromQuery] double ratingScore)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (!await _foodDeliveryService.Check(id, email)) return Forbid("user cant rate this dish"); 



                return StatusCode(await _foodDeliveryService.setRate(id,email,ratingScore));

            }

            catch (Exception e)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }


    }



    [Route("api/order/")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        private IFoodDeliveryService _foodDeliveryService;


        public OrderController(IFoodDeliveryService foodDeliveryService)
        {
            _foodDeliveryService = foodDeliveryService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderDto model)
        {
            if (!ModelState.IsValid) //Проверка полученной модели данных
            {
                return StatusCode(401, "Model is incorrect");
            }

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                await _foodDeliveryService.CreateOrder(model, email);
                return Ok();
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                
                return Ok(await _foodDeliveryService.OrderDetails(id, email));
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }


        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<OrderInfo>> Get()
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                return await _foodDeliveryService.GetOrders(email);
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }


        [Authorize]
        [HttpPost("{id}/status")]
        public async Task<IActionResult> Post(Guid id)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                return StatusCode(await _foodDeliveryService.ConfirmOrder(id, email));
            }
            catch (Exception ex)
            {
                // TODO: Добавить логирование
                throw;
                //return StatusCode(500, "Errors during registrating new user");
            }


        }




    }


    }