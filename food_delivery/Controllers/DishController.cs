using food_delivery.Data.Models;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace food_delivery.Controllers
{
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

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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

            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }

           
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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
                    GetDishes(categories, sorting, page, pageSize, vegetarian);
            }

            catch (BadHttpRequestException)
            {

                return StatusCode(400, "Bad Request");
            }

            
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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



                return StatusCode(await _foodDeliveryService.setRate(id, email, ratingScore));

            }

            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }

            catch (KeyNotFoundException)
            {

                return StatusCode(404, "Dish not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }


        }


    }
}
