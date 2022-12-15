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
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;


namespace food_delivery.Controllers
{



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
            catch (UnauthorizedAccessException)
            {
              
                
                return StatusCode(401, "Unathorized");
            }
            
            catch(Exception ex) 
            {
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPost("dish/{dishId}"), Authorize]

        public async Task<IActionResult> Post(Guid dishId)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
                
                return StatusCode(await _foodDeliveryService.addDish(dishId, email));
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


        [HttpDelete, Authorize]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> Delete(Guid dishId, bool increase = false)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;
               
                return StatusCode(await _foodDeliveryService.deleteDish(dishId, increase, email));
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