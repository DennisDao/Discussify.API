using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.UserAggegrate;
using Domain.AggegratesModel.UserAggegrate.DomainException;
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

        public IEnumerable<IUser> FindUser(string query)
        {
            return _userManager.Users.Where(x => (x.Email.StartsWith(query) || x.Email.Contains(query))
                                                || x.FirstName.StartsWith(query) || x.FirstName.Contains(query)
                                                || x.LastName.StartsWith(query) || x.LastName.Contains(query)).ToList();
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

        public IEnumerable<IUser> GetUsers(int pageSize, int pageNumber)
        {
            var users = _userManager.Users
                .OrderByDescending(x => x.WhenCreated)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return users;
        }
    }
}
