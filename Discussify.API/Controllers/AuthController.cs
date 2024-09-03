using Discussify.API.Models;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Discussify.API.Extensions;
using Domain.AggegratesModel.UserAggegrate.DomainException;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationSerive;
        private readonly IServer _server;

        public AuthController(IAuthenticationService authenticationService, IServer server)
        {
            _authenticationSerive = authenticationService;
            _server = server;
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
                authResponse.Avatar = $"{_server.GetHostUrl()}/Avatars/{authResponse.Avatar}";
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
