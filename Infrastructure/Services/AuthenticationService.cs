using CommonDataContract;
using Domain.AggegratesModel.UserAggegrate;
using Infrastructure.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly ApplicationDbContext _context;

        /// <summary> Static members </summary>
        private static byte[] _secret = Encoding.ASCII.GetBytes("MySuperSecretSuperSecretPassword");
        private static TokenValidationParameters _tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        public AuthenticationService(UserManager<ApplicationUser> userManager, 
            IUserRepository userRepository,
            IOptions<JWTSettings> jwtSettings,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<TokenResponse> Login(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            var isPasswordMatch = await _userManager.CheckPasswordAsync(user as ApplicationUser, password);

            if (isPasswordMatch == false) 
            {
                throw new Exception("Unauthorized!");
            }
           
            JwtSecurityToken token = GenerateJwtToken(user as ApplicationUser);
            RefreshToken refreshToken = await GenerateRefreshToken(user as ApplicationUser, token);

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken.Token,
                UserId = user.Id,
                Avatar = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<TokenResponse> RefreshToken(string token, string refreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
            var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);

            if(storedToken == null)
            {
                throw new Exception("Refresh token not found");
            }

            try
            {
                var checkResult = jwtTokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                if (storedToken.WhenExpire >= DateTime.UtcNow) 
                {
                    // Refresh token is not expired
                    JwtSecurityToken jwtToken = GenerateJwtToken(user as ApplicationUser);
                    return new TokenResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        Expiration = jwtToken.ValidTo,
                        RefreshToken = storedToken.Token,
                        UserId = user.Id,
                        Avatar = user.Avatar,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                }
                else
                {
                    JwtSecurityToken jwtToken = GenerateJwtToken(user as ApplicationUser);
                    RefreshToken newRefreshToken = await GenerateRefreshToken(user as ApplicationUser, jwtToken);
                    return new TokenResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        Expiration = jwtToken.ValidTo,
                        RefreshToken = newRefreshToken.Token,
                        UserId = user.Id,
                        Avatar = user.Avatar,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                }
            }

            JwtSecurityToken newToken = GenerateJwtToken(user as ApplicationUser);
            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newToken),
                Expiration = newToken.ValidTo,
                RefreshToken = storedToken.Token,
                UserId = user.Id,
                Avatar = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        private JwtSecurityToken GenerateJwtToken(ApplicationUser? user)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(20),
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
