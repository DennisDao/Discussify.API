using Domain.SeedWork;

namespace Domain.AggegratesModel.UserAggegrate
{
    public sealed class User : Entity, IAggregateRoot, IUser
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Avatar { get; private set; }
        public string Email { get; private set; }
        public string? Bio { get; private set; }
        public DateTime WhenCreated { get; private set; }
        public DateTime WhenUpdated { get; private set; }

        private User()
        {
                
        }

        public static User Create(string firstName, string lastName, string email)
        {
            return new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                WhenUpdated = DateTime.UtcNow,
                WhenCreated = DateTime.UtcNow,
            };
        }

        public static User Create(IUser user)
        {
            return new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar,
                WhenUpdated = user.WhenCreated,
                WhenCreated = user.WhenUpdated,
            };
        }

        public void ChangeFirstName(string firstName)
        {
            FirstName = firstName;
        }
    }
}
