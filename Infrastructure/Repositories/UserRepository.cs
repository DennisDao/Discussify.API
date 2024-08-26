using Domain.AggegratesModel.UserAggegrate;
using Domain.Exception;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IUser> GetUserByEmailAsync(string email)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new UserNotFoundDomainException("User not found");
            }

            return user;
        }

        public async Task<IUser> GetUserByIdAsync(int id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new UserNotFoundDomainException("User not found");
            }

            return user;
        }
    }
}
