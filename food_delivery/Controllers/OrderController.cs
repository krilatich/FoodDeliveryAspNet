using food_delivery.Data.Models;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace food_delivery.Controllers
{
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
               
                return StatusCode(await _foodDeliveryService.CreateOrder(model, email));
            }
            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }
            catch (KeyNotFoundException)

            {

                return StatusCode(403, "Forbid");

            }
           
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id)
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                if (await _foodDeliveryService.OrderDetails(id, email) == null) return BadRequest("not correct id");

                return Ok(await _foodDeliveryService.OrderDetails(id, email));
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


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderInfo>>> Get()
        {

            try
            {
                string email = User.FindFirst(ClaimTypes.Name)?.Value;

                return Ok(await _foodDeliveryService.GetOrders(email));
            }
            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }

            catch (NullReferenceException)
            {

                return StatusCode(404, "Orders not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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
            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }

            catch (NullReferenceException)
            {

                return StatusCode(404, "Order not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }


        }




    }
}
