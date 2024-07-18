using Domain.AggegratesModel.UserAggegrate;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<int>, IUser
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Avatar { get; private set; }
        public string Email { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }

        public static ApplicationUser Create(string firstName, string lastName, string email)
        {
            return new ApplicationUser() 
            {
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Avatar = "test", // Fix this avatar should be optional
                WhenCreated = DateTime.UtcNow,
                WhenUpdated = DateTime.UtcNow 
            };
        }

        public static User ToDomainUser(IUser user)
        {
            return User.Create(user);
        }
    }
}
