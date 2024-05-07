using Domain.AggegratesModel.UserAggegrate;
using Domain.Exception;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            var post = _context.Posts
                            .Include(x => x.Topics)
                            .ThenInclude(x => x.TopicType)
                            .FirstOrDefault(x => x.Id == 1);

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
