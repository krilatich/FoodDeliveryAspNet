using food_delivery.Data.Models;
using food_delivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace food_delivery.Controllers
{

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
                    token = encodedJwt,
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
                
                return StatusCode(500, "Errors during registrating new user");
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
            catch (UnauthorizedAccessException)
            {

                return StatusCode(401, "Unathorized");
            }

            
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
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
                
                return StatusCode(500, "Internal Server error");
            }
        }
    }

}
