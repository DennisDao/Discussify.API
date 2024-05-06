using Discussify.API.Models;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Discussify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IOptions<JWTSettings> jwtSettings,
            ApplicationDbContext context, 
            IConfiguration config)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _context = context;
            _config = config;

            var password = _config.GetSection("JwtSettings.Secret").ToString();

            var secret = Encoding.ASCII.GetBytes("MySuperSecretSuperSecretPassword");

            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            JwtSecurityToken token = GenerateJwtToken(user);
            RefreshToken refreshToken = await GenerateRefreshToken(user, token);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                refreshToken = refreshToken.Token,
                userId = user.Id,
                user.Avatar,
                user.FirstName,
                user.LastName
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid token refresh request");
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.RefreshToken);
            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (storedToken != null) 
            {
                try
                {
                    var checkResult = jwtTokenHandler.ValidateToken(model.Token, _tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (SecurityTokenExpiredException ex) 
                { 
                    if(storedToken.WhenExpire >= DateTime.UtcNow) // Not expired
                    {
                        JwtSecurityToken token = GenerateJwtToken(user);
                        var response = new
                        {
                            token = jwtTokenHandler.WriteToken(token),
                            expiration = storedToken.WhenExpire,
                            refreshToken = storedToken.Token,
                            userId = user.Id,
                            user.Avatar,
                            user.FirstName,
                            user.LastName
                        };

                        return Ok(response);
                    }
                    else
                    {
                        JwtSecurityToken token = GenerateJwtToken(user);
                        RefreshToken refreshToken = await GenerateRefreshToken(user, token);
                        return Ok(new
                        {
                            token = jwtTokenHandler.WriteToken(token),
                            expiration = token.ValidTo,
                            refreshToken = refreshToken.Token,
                            userId = user.Id,
                            user.Avatar,
                            user.FirstName,
                            user.LastName
                        });
                    }
                }
            }

            return BadRequest("Invalid token refresh request");
        }

        private JwtSecurityToken GenerateJwtToken(ApplicationUser? user)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private async Task<RefreshToken> GenerateRefreshToken(ApplicationUser? user, JwtSecurityToken token)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                WhenCreated = DateTime.UtcNow,
                WhenExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
    }
}
