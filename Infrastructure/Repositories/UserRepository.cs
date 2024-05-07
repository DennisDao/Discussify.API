using Domain.AggegratesModel.UserAggegrate;
using Domain.Exception;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UserNotFoundDomainException("User not found");
            }

            return user;
        }

        public async Task<IUser> GetUserByIdAsync(string id)
        {
           var user = await _userManager.FindByIdAsync(id);

           if (user == null) 
           {
                throw new UserNotFoundDomainException("User not found");
           }

           return user;
        }
    }
}
