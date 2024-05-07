using CommonDataContract;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> Login(string email, string password);

        Task<TokenResponse> RefreshToken(string token, string refreshToken);
    }
}
