using Domain.Exception;
using Discussify.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Infrastructure.Services.IAuthenticationService _authenticationSerive;

        public AuthController(Infrastructure.Services.IAuthenticationService authenticationService)
        {
            _authenticationSerive = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                var authResponse = await _authenticationSerive.Login(model.Email, model.Password);
                return Ok(authResponse);
            }
            catch(UserNotFoundDomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid token refresh request");
            }

            try
            {
                var token = await _authenticationSerive.RefreshToken(model.Token, model.RefreshToken);

                if (token != null)
                {
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Invalid token refresh request");
                }
            }
            catch (UserNotFoundDomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
