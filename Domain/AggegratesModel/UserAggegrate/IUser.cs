using Domain.SeedWork;

namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUser : IAggregateRoot
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
