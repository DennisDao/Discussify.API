using Domain.SeedWork;

namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUser 
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Avatar { get; }
        string Email { get; }
        DateTime WhenCreated { get; }
        DateTime WhenUpdated { get; }
    }
}
