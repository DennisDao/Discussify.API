using Domain.SeedWork;

namespace Domain.AggegratesModel.UserAggegrate
{
    public interface IUser 
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string? Avatar { get; }
        string Email { get; }
        string? Bio { get; }
        DateTime WhenCreated { get; }
        DateTime WhenUpdated { get; }
    }
}
